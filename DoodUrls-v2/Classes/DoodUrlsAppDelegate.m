//
//  DoodUrlsAppDelegate.m
//  DoodUrls
//
//  Created by Matt Emson on 11/09/2009.
//  Copyright Rat Cow Software 2009. All rights reserved.
//

#import "DoodUrlsAppDelegate.h"
#import "DoodUrlsViewController.h"

@implementation DoodUrlsAppDelegate

@synthesize window;
@synthesize viewController;


- (void)applicationDidFinishLaunching:(UIApplication *)application {    
    
    // Override point for customization after app launch    
    [window addSubview:viewController.view];
    [window makeKeyAndVisible];
}


- (void)dealloc {
    [viewController release];
    [window release];
    [super dealloc];
}


@end
