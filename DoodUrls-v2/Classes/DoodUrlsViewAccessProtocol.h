/*
 *  DoodUrlsViewAccessProtocol.h
 *  DoodUrls
 *
 *  Created by Matt Emson on 23/09/2009.
 *  Copyright 2009 __MyCompanyName__. All rights reserved.
 *
 */

@protocol DoodUrlsViewAccessor
-(void)doLoadImages;
-(void)feedLoadErrorWithLabel: (NSString*) label andAlertMessage: (NSString*) message;
-(void)setProgressMeterActiveTo:(BOOL) active;
-(void)setTheImage:(UIImage*)image  withCaption: (NSString*) caption;
@end

@interface DoodUrlsViewAccess : NSObject <DoodUrlsViewAccessor>
{

}

@end


