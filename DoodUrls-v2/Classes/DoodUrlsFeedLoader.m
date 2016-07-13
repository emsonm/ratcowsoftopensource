//
//  DoodUrlsFeedLoader.m
//  DoodUrls
//
//  Created by Matt Emson on 12/09/2009.
//  Copyright 2009  Rat Cow Software. All rights reserved.
//

#import "DoodUrlsFeedLoader.h"


@implementation DoodUrlsFeedLoader

-(id)init:(id)adelegate;
{
	self = [super init];
	
	delegate = adelegate;
	
	images = nil; //[[NSMutableArray alloc] init]; 
	
	return self;
}

-(void)dealloc
{
	if (images != nil)
	{
		[images release];
	}
	[super dealloc];
}

-(void)addHolder: (DoodUrlsHolder *) aholder
{
	[images addObject: aholder];
}

-(DoodUrlsHolder*) getHolderAtIndex: (int) aindex
{
	DoodUrlsHolder* resultant = [images objectAtIndex:aindex];
	//[resultant retain];
	return resultant;
}

-(int)count
{
	return [images count];
}

//this is a translation of the routine in the ActionScript version
NSMutableArray* GetInfo(NSString* data)
{
	NSMutableArray* result = [[NSMutableArray alloc] init]; 
	
	
	bool _in_item = false;
	
	NSMutableString* url = [[NSMutableString alloc] initWithString:@""]; 
	NSMutableString* description = [[NSMutableString alloc] initWithString:@""]; 
	
	NSUInteger a  = 0;
	
	for (NSUInteger i = 0; i < [data length]; i++)
	{
		
		if ([data characterAtIndex:i] == '<')
		{
			if([data characterAtIndex:i+1] == 'i' && [data characterAtIndex:i+2] == 't' && [data characterAtIndex:i+3] == 'e' && [data characterAtIndex:i+4] == 'm' && [data characterAtIndex:i+5] == '>')
			{
				i+= 5;
				_in_item = true;
				[description setString: @""];
				[url setString: @""];
			}
		}
		else if([data characterAtIndex:i] == '/')
		{
			if([data characterAtIndex:i+1] == 'i' && [data characterAtIndex:i+2] == 't' && [data characterAtIndex:i+3] == 'e' && [data characterAtIndex:i+4] == 'm' && [data characterAtIndex:i+5] == '>')
			{
				i+= 5;
				_in_item = false;
			}
		}
		
		if (_in_item)
		{
			if ([data characterAtIndex:i] == '<')
			{
				if ([data characterAtIndex:i + 1] == 't' && [data characterAtIndex:i + 2] == 'i' && [data characterAtIndex:i + 3] == 't' && [data characterAtIndex:i + 4] == 'l' && [data characterAtIndex:i + 5] == 'e' && [data characterAtIndex:i + 6] == '>')
				{
					i += 7;
					
					while ([data characterAtIndex:i] != '<')
					{
						[description appendString: [NSString stringWithFormat: @"%C", [data characterAtIndex:i]]];
						i++;
					}
				}
			}
			
			
			//src="
			if ([data characterAtIndex:i] == 's' && [data characterAtIndex:i + 1] == 'r' && [data characterAtIndex:i + 2] == 'c' && [data characterAtIndex:i + 3] == '=' && [data characterAtIndex:i + 4] == '&')
			{
				i += 10;
				
				while (!([data characterAtIndex:i] == '&' && [data characterAtIndex:i + 1] == 'q' && [data characterAtIndex:i + 2] == 'u' && [data characterAtIndex:i + 3] == 'o' && [data characterAtIndex:i + 4] == 't' && [data characterAtIndex:i + 5] == ';'))
				{
					//url = url + [data characterAtIndex:i];
					[url appendString: [NSString stringWithFormat: @"%C", [data characterAtIndex:i]]];
					i++;
				}
				
				NSRange range = [url rangeOfString:@".jpg"]; // options:NSCaseInsensitiveSearch];
				if (range.location != NSNotFound)
				{
					
					
					if ([description isEqualToString: @""] == NO)
					{
						//result.push(new Holder(description, url));
						DoodUrlsHolder* holder = [[DoodUrlsHolder alloc] initWithURLs:url andDescription:description];
						[result addObject:holder];
					}
					else
					{
						//result.push(new Holder("Unknown " + a.toString(), url));
						DoodUrlsHolder* holder = [[DoodUrlsHolder alloc] initWithURLs:url andDescription:@"Unknown"];
						[result addObject:holder];
						a++;
					}
					
				}
				
				[url setString:@""];
			}
			
		}
	}
	
	[description release];
	[url release];
	return result;
}


//////////////////////////////////////////////////

//todo: make it so....
-(void)loadAsync: (NSString *) aurl
{
	[delegate setProgressMeterActiveTo:YES];
	
	NSURL* theUrl = [NSURL URLWithString:aurl];
	
	if (connection!=nil) { [connection release]; }
    if (data!=nil) { [data release]; }
    NSURLRequest* request = [NSURLRequest requestWithURL:theUrl
											 cachePolicy:NSURLRequestReturnCacheDataElseLoad
										 timeoutInterval:60.0];
    connection = [[NSURLConnection alloc]
				  initWithRequest:request delegate:self];
	
}

- (void)connection:(NSURLConnection *)theConnection didReceiveData:(NSData *)incrementalData 
{
    if (data==nil) {
		data = [[NSMutableData alloc] initWithCapacity:2048];
    }
    [data appendData:incrementalData];
}

- (void)connectionDidFinishLoading:(NSURLConnection*)theConnection {
	
    [connection release];
    connection=nil;
	
	NSString * feeddata = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];	
	[data release];
	data = nil;
	
	[self readDataFromFeedString: feeddata];
	[delegate setProgressMeterActiveTo:NO];
	[delegate doLoadImages];
}

- (void)connection:(NSURLConnection *)connection didFailWithError:(NSError *)error
{
	[delegate feedLoadErrorWithLabel:@"Data could not be downloaded" andAlertMessage:@"The DoodUrls.com feed data could not be retrieved"];
    [delegate setProgressMeterActiveTo:NO];
}





/////////////////////////////////////////////////


//todo: make it so....
-(void)load: (NSString *) aurl
{	
	NSURL* theUrl = [NSURL URLWithString:aurl];
	@try {
		
		NSString* feeddata = [[NSString alloc] initWithContentsOfURL: theUrl];
		
		[self readDataFromFeedString: feeddata];
		[delegate doLoadImages];
		
	}
	@catch (NSException * e) {
		//something went wrong!!
		NSLog(@"Something went wrong! 2");
	}
	@finally {
		//[theUrl release];
	}
}

//this is new!!
-(void) readDataFromFeedString: (NSString*) feedString
{
	@try {
		
		//attempt to be a good citizen!
		if (images != nil && [images count] > 0)
		{
			[images removeAllObjects];
		}
		
		//this probably needs a try round it...
		//NSMutableArray* temp = GetInfo(data);
		//[images addObjectsFromArray:temp];
		//[temp release];
		images = GetInfo(feedString);
		[images retain];
	}
	@catch (NSException * e) {
		//something went wrong!!
		NSLog(@"Something went wrong! 1");
	}
	@finally {
		//[data release];
	}
}


-(void)releaseImagesExcept: (int) aindex
{
	NSLog(@"Releasing images");
	for(int i = 0; i < [images count]; i++)
	{
		if (i == aindex) continue;
		else
		{
			DoodUrlsHolder* holder = [self getHolderAtIndex:i];
			if (holder.loaded == YES)
			{
				@try {
					[holder.image release];
					//holder.image = nil;
				}
				@catch (NSException * e) {
					NSLog(@"at least one image didn't release.. bummer");
				}
				@finally {
					holder.loaded = NO;
				}
			}
		}
	}
	NSLog(@"Images released");
}

@end
