#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import "DoodUrlsHolder.h"
#import "DoodUrlsFeedLoader.h"
#import "AdMobDelegateProtocol.h"
#import "DoodUrlsViewAccessProtocol.h"

#define AD_REFRESH_PERIOD 60.0 // display fresh ads once per minute

@class AdMobView;

@interface DoodUrlsViewController : UIViewController <UIAlertViewDelegate,  AdMobDelegate, DoodUrlsViewAccessor>
{
    IBOutlet UIImageView *imageView;
    IBOutlet UIBarButtonItem *nextButton;
    IBOutlet UIBarButtonItem *priorButton;
    IBOutlet UIActivityIndicatorView *progressSpinner;
    IBOutlet UIBarButtonItem *refreshButton;
	IBOutlet UILabel *imageLabel;
	
	DoodUrlsFeedLoader *feed;
	int displayedImage;
	
	AdMobView *adMobAd;
	NSTimer *refreshTimer;
	NSTimer *initTimer;
	
}
@property (nonatomic, retain) UIImageView *imageView;
@property (nonatomic, retain) UIBarButtonItem *nextButton;
@property (nonatomic, retain) UIBarButtonItem *priorButton;
@property (nonatomic, retain) UIBarButtonItem *refreshButton;
@property (nonatomic, retain) UILabel *imageLabel;
@property (nonatomic, retain) UIActivityIndicatorView *progressSpinner;

-(void) dealloc;

- (IBAction)nextImage:(id)sender;
- (IBAction)priorImage:(id)sender;
- (IBAction)refreshImagesFromFeed:(id)sender;

-(void)loadImageAtIndex:(int)aindex;
-(void)alertOKCancelAction;
-(void)dialogSimpleAction;
-(void)doLoadImages;

-(void)loadDelayInitialFeed:(NSTimer *)timer;
-(void)startAdTimer;

- (void)viewDidLoad;
@end
