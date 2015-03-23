#Transit

Transit is a framework that allows components to talk to each other similar to how [signals and slots](http://en.wikipedia.org/wiki/Signals_and_slots) work. Using Transit, components can communicate with one or more other components with a minimal amount of code.

####How it works

At its core, Transit is made up of components. A component can have either RouteIn or RouteOut, or both, attributes attached to properties, methods, and events. The communication layer between the components is handled by different route types. Transit supports four different route types:

* `PropertyToPropertyRoute`
* `PropertyToMethodRoute`
* `EventToPropertyRoute`
* `EventToMethodRoute`

Transit can also be extended to incorporate other route types. The main container for Transit is the `Package` object. The `Package` contains all of the components and routes in use by that `Package`. The `Package` is also responsible for registering and unregistering routes.

####Route Converters

Simples scenarios let components talk to each other directly, however, more complex scenarios might have components with different types talking to each other. In these cases Transit uses a `RouteConverter`. A `RouteConverter` is the middleman in the route and informs components id their types can be converted, and also automatically does the converting of types when communication happens.

####Controlling Route Registration

Transit employs eight events to help with route registration. These events allow a developer to cancel registration and unregistration, and also to monitor when registration and unregistration occur.

####Using Transit

Please see the example projects in the solution for help using Transit. 

#####Documentation

As of now, documentation is minimal. The plan is to get the code documented and then to create a more formal document for the API. Please use the examples in the solution for now as your best source for documentation and example use of the API.

_NOTE: Transit is in the early stages of development. This release is stable, but expect some bugs to be present._
