//
//  DoodUrlsFeedLoader.h
//  DoodUrls
//
//  Created by Matt Emson on 12/09/2009.
//  Copyright 2009  Rat Cow Software. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import "DoodUrlsHolder.h"
#import "DoodUrlsViewAccessProtocol.h"

@class DoodUrlsViewController;

@interface DoodUrlsFeedLoader : NSObject {
	NSMutableArray *images;
	
	//async vars
	NSURLConnection* connection;
    NSMutableData* data;
	DoodUrlsViewAccess* delegate;
}

-(id)init: (id)adelegate;
-(void)dealloc;

-(void)addHolder: (DoodUrlsHolder *) aholder;
-(DoodUrlsHolder*) getHolderAtIndex: (int) aindex;
-(int)count;
-(void)load: (NSString *) aurl;
-(void)loadAsync: (NSString *) aurl;

-(void)releaseImagesExcept: (int) aindex;
-(void)readDataFromFeedString: (NSString*) feedString;




@end
