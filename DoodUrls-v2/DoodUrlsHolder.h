//
//  DoodUrlsHolder.h
//  DoodUrls
//
//  Created by Matt Emson on 12/09/2009.
//  Copyright 2009 Rat Cow Software. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import "DoodUrlsViewAccessProtocol.h"

/*
 public var url: String;
 public var description: String;
 
 public function Holder( adescription: String,  aurl: String)
 {
 super();
 url = aurl;
 description = adescription;
 
 }
 
 */


@interface DoodUrlsHolder : NSObject {
	
	NSString *url;
	NSString *description;
	BOOL loaded;
	UIImage *image;
	
	//async vars
	NSURLConnection* connection;
    NSMutableData* data;
	DoodUrlsViewAccess* delegate;
}

@property (nonatomic, retain) NSString *url;
@property (nonatomic, retain) NSString *description;
@property (nonatomic) BOOL loaded;
@property (nonatomic, retain) UIImage *image;

- (id) init;
- (id) initWithURLs: (NSString *) aurl andDescription: (NSString *) adescription;
- (void) dealloc;
- (BOOL) loadImage: (id) adelegate withAsyncCall: (BOOL) doUseAsyncCall;


@end
