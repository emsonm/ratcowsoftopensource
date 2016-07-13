//
//  DoodUrlsAppDelegate.h
//  DoodUrls
//
//  Created by Matt Emson on 11/09/2009.
//  Copyright  Rat Cow Software 2009. All rights reserved.
//

#import <UIKit/UIKit.h>

@class DoodUrlsViewController;

@interface DoodUrlsAppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
    DoodUrlsViewController *viewController;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet DoodUrlsViewController *viewController;

@end

