# Car Drivers

It is a research project on a parallel solution handling moving items in mesh with no empty spaces, with a rule that a place is space can be taken only by a single item.

The problem was to handle situations where in a ring of fields like

* `AB`
* `CD`

all items have intensions to swap space with each other in a manner of `A`->`B` ; `B`->`D`; `D`->`C`; `C`->`A`; effectively rotating fields clockwise.

The trivial solution fails here as there is never any 'free' spot to take over, so it is hard to, up front, predict if a move is possible. 

The solution needed to be designed with a parallelism in mind, meaning that every slot only has its own intensions to share with the simulator, so it is 'aware' of as small context as possible.

The simulator's responsibility (here World::Simulate) was to grab all the intensions and decide which moves are possible.