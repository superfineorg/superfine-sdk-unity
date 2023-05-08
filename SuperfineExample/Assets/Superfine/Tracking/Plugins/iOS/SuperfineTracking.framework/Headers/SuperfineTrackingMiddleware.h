#import <Foundation/Foundation.h>
#import "SuperfineTrackingContext.h"

typedef void (^SuperfineTrackingMiddlewareNext)(SuperfineTrackingContext *_Nullable newContext);

@protocol SuperfineTrackingMiddleware
@required

// NOTE: If you want to hold onto references of context AFTER passing it through to the next
// middleware, you should explicitly create a copy via `[context copy]` to guarantee
// that it does not get changed from underneath you because contexts can be implemented
// as mutable objects under the hood for performance optimization.
// The behavior of keeping reference to a context AFTER passing it to the next middleware
// is strictly undefined.

// Middleware should **always** call `next`. If the intention is to explicitly filter out
// events from downstream, call `next` with `nil` as the param.
// It's ok to save next callback until a more convenient time, but it should always always be done.
// We'll probably actually add tests to sure it is so.
// TODO: Should we add error as second param to next?
- (void)context:(SuperfineTrackingContext *_Nonnull)context next:(SuperfineTrackingMiddlewareNext _Nonnull)next;

@end

typedef void (^SuperfineTrackingMiddlewareBlock)(SuperfineTrackingContext *_Nonnull context, SuperfineTrackingMiddlewareNext _Nonnull next);


@interface SuperfineTrackingBlockMiddleware : NSObject <SuperfineTrackingMiddleware>

@property (nonnull, nonatomic, readonly) SuperfineTrackingMiddlewareBlock block;

- (instancetype _Nonnull)initWithBlock:(SuperfineTrackingMiddlewareBlock _Nonnull)block;

@end


typedef void (^RunMiddlewaresCallback)(BOOL earlyExit, NSArray<id<SuperfineTrackingMiddleware>> *_Nonnull remainingMiddlewares);

// XXX TODO: Add some tests for SuperfineTrackingMiddlewareRunner
@interface SuperfineTrackingMiddlewareRunner : NSObject

// While it is certainly technically possible to change middlewares dynamically on the fly. we're explicitly NOT
// gonna support that for now to keep things simple. If there is a real need later we'll see then.
@property (nonnull, nonatomic, readonly) NSArray<id<SuperfineTrackingMiddleware>> *middlewares;

- (void)run:(SuperfineTrackingContext *_Nonnull)context callback:(RunMiddlewaresCallback _Nullable)callback;

- (instancetype _Nonnull)initWithMiddlewares:(NSArray<id<SuperfineTrackingMiddleware>> *_Nonnull)middlewares;

@end
