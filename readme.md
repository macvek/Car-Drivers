# Car Drivers

It is a research project on a parallel solution handling moving items in mesh with no empty spaces, with a rule that a place is space can be taken only by a single item.

The problem was to handle situations where in a ring of fields like

* `AB`
* `CD`

https://github.com/user-attachments/assets/9465ea65-fbac-4caa-a584-bd04f91618c9

all items have intensions to swap space with each other in a manner of `A`->`B` ; `B`->`D`; `D`->`C`; `C`->`A`; effectively rotating fields clockwise.

The trivial solution fails here as there is never any 'free' spot to take over, so it is hard to, up front, predict if a move is possible. 

The solution needed to be designed with a parallelism in mind, meaning that every slot only has its own intensions to share with the simulator, so it is 'aware' of as small context as possible.

The simulator's responsibility (here World::Simulate) was to grab all the intensions and decide which moves are possible.

## Projects

* P2 is a solution which grabs P2 (UI), P2Classes (logic), and P2Tests ( MSTests )
* P2 - WinForms - ui part created to visualize steps of a simulation
* P2Classes - single CarDrivers class
* P2Tests - CarDriversTests class containing all unit tests against CarDrivers class



