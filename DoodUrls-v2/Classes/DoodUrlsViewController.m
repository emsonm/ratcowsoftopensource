#import "DoodUrlsViewController.h"
#import "AdMobView.h"

@implementation DoodUrlsViewController

@synthesize imageView;
@synthesize nextButton;
@synthesize priorButton;
@synthesize refreshButton;
@synthesize imageLabel;
@synthesize progressSpinner;


-(void)dealloc
{
	[adMobAd release];
	[imageView release];
	[nextButton release];
	[priorButton release];
	[refreshButton release];
	[imageLabel release];
	[progressSpinner release];
	[feed release];
	[super dealloc];
}

- (IBAction)nextImage:(id)sender 
{
	//make sure the ads load!!!
	if (![refreshTimer isValid])
	{
		[self startAdTimer];
	}
    displayedImage++;
	if (displayedImage >= [feed count]) displayedImage = 0;
    [self loadImageAtIndex: displayedImage];
}

- (IBAction)priorImage:(id)sender 
{
	//make sure the ads load!!!
	if (![refreshTimer isValid])
	{
		[self startAdTimer];
	}
	
	displayedImage--;
	if (displayedImage < 0) displayedImage = [feed count] -1;
    [self loadImageAtIndex: displayedImage];
}

- (IBAction)refreshImagesFromFeed:(id)sender 
{
	//make sure the ads load!!!
	if (![refreshTimer isValid])
	{
		[self startAdTimer];
	}
	
	[self alertOKCancelAction];
}

//general image loader routine
-(void)loadImageAtIndex:(int)aindex
{		
	if ([feed count] == 0) return;
	
	//[progressSpinner startAnimating];
		
	DoodUrlsHolder* holder = [feed getHolderAtIndex:aindex];
	//[holder loadImage];
	if (holder.loaded == NO)
	{
		//[holder loadImage:self withAsyncCall: NO];
		[holder loadImage:self withAsyncCall: YES];
	}
	else
	{
		@try 
		{
			[self setTheImage: holder.image  withCaption: holder.description];
		}
		@catch (NSException * e) 
		{
			[imageLabel setText: @"Image wasn't loaded."];
			//attempt to force a reload
			holder.loaded = NO;
			[holder loadImage:self withAsyncCall: YES];
		}
		@finally {
			//set the description			
		}
	}
	
	//[imageView.image retain];
	
	//[imageLabel setText: [[NSString alloc] initWithString: holder.description]];
	
	//[progressSpinner stopAnimating];
}

-(void)setTheImage:(UIImage*)image withCaption: (NSString*) caption
{
	imageView.image = image;
	//set the description
	[imageLabel setText: [[NSString alloc] initWithString: caption]];
}


- (void)didReceiveMemoryWarning 
{
	// Releases the view if it doesn't have a superview.
	[super didReceiveMemoryWarning];
	
	// Release any cached data, images, etc that aren't in use.
	[feed releaseImagesExcept:displayedImage];
}



- (void)viewDidLoad
{	
	[self setProgressMeterActiveTo:YES];
	feed = [[DoodUrlsFeedLoader alloc] init:self];
	
	initTimer = [NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(loadDelayInitialFeed:) userInfo:nil repeats:NO];
	
	// Request an ad
	adMobAd = [AdMobView requestAdWithDelegate:self]; // start a new ad request
	[adMobAd retain]; // this will be released when it loads (or fails to load)
}

- (void)loadDelayInitialFeed:(NSTimer *)timer 
{
	[feed load:@"http://www.doodurls.com/feed/rss/"];
	displayedImage = 0;
	[timer release];
	[self setProgressMeterActiveTo:NO];
}

-(void) doLoadImages
{
	@try {
		///[self setProgressMeterActiveTo:YES];
		[nextButton setEnabled:NO];
		[priorButton setEnabled:NO];
		
		@try {
			[self loadImageAtIndex: displayedImage];
		}
		@catch (NSException * e) {
			//error
		}
		@finally {
			//re-enable buttons if there is more than one image in the feed
			if ([feed count] > 1)
			{
				[nextButton setEnabled:YES];
				[priorButton setEnabled:YES];
			}
			else
			{
				[self dialogSimpleAction];
			}
		}
		
	}
	@catch (NSException * e) {
		
	}
	@finally {
		//[self setProgressMeterActiveTo:NO];
		[refreshButton setEnabled:YES];
	}
}

- (void)alertOKCancelAction
{
    // open a alert with an OK and cancel button
    UIAlertView *alert = 
	[[UIAlertView alloc] 
	 initWithTitle:@"Reload data" 
	 message:@"Do you want to reload the feed data?"
	 delegate:self 
	 cancelButtonTitle:@"No" 
	 otherButtonTitles:@"Yes", 
	 nil];
    [alert show];
    [alert release];
}

- (void)feedLoadErrorWithLabel: (NSString*) label andAlertMessage: (NSString*) message
{
	UIImage *image = imageView.image;
	imageView.image = nil;
	[image release];
    
	[imageLabel setText:@"Could not retrieve data."];
	
    // open a dialog with just an OK button
	UIAlertView *alert = 
	[[UIAlertView alloc] 
	 initWithTitle:@"Error" 
	 message:@"The DoodUrls picture feed failed to load"
	 delegate:self 
	 cancelButtonTitle:@"OK" 
	 otherButtonTitles:nil,
	 nil];
    [alert show];    // show from our table view (pops up in the middle of the table)
    [alert release];
	
	[self setProgressMeterActiveTo:NO];
}

-(void)setProgressMeterActiveTo:(BOOL) active
{
	if (active)
	{
		[progressSpinner startAnimating];
	}
	else
	{
		[progressSpinner stopAnimating];
	}
}

- (void)dialogSimpleAction
{
	UIImage *image = imageView.image;
	imageView.image = nil;
	[image release];
    
	[imageLabel setText:@"Images were not found."];
	
    // open a dialog with just an OK button
	UIAlertView *alert = 
	[[UIAlertView alloc] 
	 initWithTitle:@"Error" 
	 message:@"There are no images in the feed... Are you connected to the internet?"
	 delegate:self 
	 cancelButtonTitle:@"OK" 
	 otherButtonTitles:nil,
	 nil];
    [alert show];    // show from our table view (pops up in the middle of the table)
    [alert release];
	
	[self setProgressMeterActiveTo:NO];
}

- (void)alertView:(UIAlertView *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == 1)
	{
		//[feed load:@"http://www.doodurls.com/feed/rss/"];
		//displayedImage = 0;
		//[self doLoadImages];
		
		@synchronized(self)
		{
			
			imageView.image = nil;
			imageLabel.text = @"Reloading date";
			
			[progressSpinner startAnimating];
			[refreshButton setEnabled:NO];
			
			displayedImage = 0;
            [feed load:@"http://www.doodurls.com/feed/rss/"];
		}
		
		
		//initTimer = [NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(loadDelayInitialFeed:) userInfo:nil repeats:NO];
	}
}

// Request a new ad. If a new ad is successfully loaded, it will be animated into location.
- (void)refreshAd:(NSTimer *)timer 
{
	[adMobAd requestFreshAd];
}

#pragma mark -
#pragma mark AdMobDelegate methods

- (NSString *)publisherId 
{
	return @"a14aa521551afd9"; // this should be prefilled; if not, get it from www.admob.com
}

- (UIColor *)adBackgroundColor 
{
	return [UIColor colorWithRed:0.851 green:0.89 blue:0.925 alpha:1]; // this should be prefilled; if not, provide a UIColor
}

- (UIColor *)primaryTextColor 
{
	return [UIColor colorWithRed:0.298 green:0.345 blue:0.416 alpha:1]; // this should be prefilled; if not, provide a UIColor
}

- (UIColor *)secondaryTextColor 
{
	return [UIColor colorWithRed:0.298 green:0.345 blue:0.416 alpha:1]; // this should be prefilled; if not, provide a UIColor
}

- (BOOL)mayAskForLocation 
{
	return NO; // this should be prefilled; if not, see AdMobProtocolDelegate.h for instructions
}

// To receive test ads rather than real ads...

//This is probably more polite thak fakely serving ads to myself whilst debugging ;-)
- (BOOL)useTestAd {
#if DEBUG
	return YES;
#else
	return NO;
#endif
}

/*
 - (NSString *)testAdAction {
 return @"url"; // see AdMobDelegateProtocol.h for a listing of valid values here
 }
 */ 

// Sent when an ad request loaded an ad; this is a good opportunity to attach
// the ad view to the hierachy.
- (void)didReceiveAd:(AdMobView *)adView {
	NSLog(@"AdMob: Did receive ad");
	// get the view frame
	CGRect frame = self.view.frame;
	
	// put the ad at the bottom of the screen
	adMobAd.frame = CGRectMake(0, frame.size.height - 48, frame.size.width, 48);
	
	[self.view addSubview:adMobAd];
	//[refreshTimer invalidate];
	//refreshTimer = [NSTimer scheduledTimerWithTimeInterval:AD_REFRESH_PERIOD target:self selector:@selector(refreshAd:) userInfo:nil repeats:YES];
	[self startAdTimer];
}

//call this to restart the time....
-(void)startAdTimer
{
	[refreshTimer invalidate];
	refreshTimer = [NSTimer scheduledTimerWithTimeInterval:AD_REFRESH_PERIOD target:self selector:@selector(refreshAd:) userInfo:nil repeats:YES];
}

//static int failedcount = 0;

// Sent when an ad request failed to load an ad
- (void)didFailToReceiveAd:(AdMobView *)adView {
	NSLog(@"AdMob: Did fail to receive ad");
	[adMobAd release];
	adMobAd = nil;
	// we could start a new ad request here, but in the interests of the user's battery life, let's not
	
    [self startAdTimer];
}




@end
