//
//  DoodUrlsHolder.m
//  DoodUrls
//
//  Created by Matt Emson on 12/09/2009.
//  Copyright 2009  Rat Cow Software. All rights reserved.
//

#import "DoodUrlsHolder.h"


@implementation DoodUrlsHolder

@synthesize url;
@synthesize description;
@synthesize loaded;
@synthesize image;

- (id) init
{
	self = [super init];
	image = nil;
	loaded = NO;
	return self;
}

- (id) initWithURLs: (NSString *) aurl andDescription: (NSString *) adescription
{
	self = [super init];
	url = [NSString stringWithString: aurl];
	[url retain];
	description = [NSString stringWithString: adescription];
	[description retain];
	image = nil;
	loaded = NO;
	return self;
}

- (void) dealloc
{
	[description release];
	[url release];
	if (loaded)
	{
		[image release];
	}
	
	[super dealloc];
}

- (BOOL) loadImage: (id) adelegate withAsyncCall: (BOOL) doUseAsyncCall
{
	delegate = adelegate;
	
	if (doUseAsyncCall)
	{
		if (loaded)
		{
			[delegate setTheImage: image withCaption: [self description]];
		}
		else
		{
			[delegate setProgressMeterActiveTo:YES];
			
			NSURL* theUrl = [NSURL URLWithString:self.url];
			
			if (connection!=nil) { [connection release]; }
			if (data!=nil) { [data release]; }
			NSURLRequest* request = [NSURLRequest requestWithURL:theUrl
													 cachePolicy:NSURLRequestReturnCacheDataElseLoad
												 timeoutInterval:60.0];
			connection = [[NSURLConnection alloc]
						  initWithRequest:request delegate:self];
		}
		return YES;
	}
	else
	{
		@try {
			image = [UIImage imageWithData: [NSData dataWithContentsOfURL: [NSURL URLWithString: self.url]]];
			[image retain];
			loaded = YES;
			[delegate setTheImage: image withCaption: [self description]];
			return YES;
		}
		@catch (NSException * e) {
			image = [UIImage imageWithContentsOfFile:@"noimage.png"];
			[image retain];
			loaded = YES;
			[delegate setTheImage: image  withCaption: @"Image wan not loaded"];
			return NO;
		}
		@finally
		{
			return YES;
		}
	}
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
	
	//NSString * feeddata = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];	
    @try {
		image = [UIImage imageWithData: data];
		[image retain];
		loaded = YES;
		[delegate setTheImage: image  withCaption: [self description]];
		return;
	}
	@catch (NSException * e) {
		image = [UIImage imageWithContentsOfFile:@"noimage.png"];
		[image retain];
		loaded = YES;
		[delegate setTheImage: image  withCaption: [self description]];
		return;
	}
	@finally
	{
		[delegate setProgressMeterActiveTo:NO];
		delegate = nil;
		return;
	}	
}

- (void)connection:(NSURLConnection *)connection didFailWithError:(NSError *)error
{
	[delegate feedLoadErrorWithLabel:@"Image could not be downloaded" andAlertMessage:@"The image from DoodUrls.com could not be retrieved"];
    [delegate setProgressMeterActiveTo:NO];
	delegate = nil;
}

@end
