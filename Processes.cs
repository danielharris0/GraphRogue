/*
 
Processes are divided into *Actions* and *Reactions^

All processes have a *Precondition* and an *Effect*.

    Actions are chosen by the player to happen, provided the precondition holds.
    Reactions happen automatically as soon as the precondition holds.
        But what if more than one reaction is applicable?
            Each reaction has a unique order number.
            If instances of the same reaction are applicable, the reaction-type's internal logic must decide.

All processes are listed in a big list somewhere, but also can specify other methods of user-discovery in the form of 'hints', attached to:
    - certain edge-types
    - certain node-types
    - 

Actions can be performed from their main listing, or from a hint.
 
*/

using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public class Change {

}

public class Process {
    //precondition
    Graph precondition;

    //result
    Change result;

    public Process(Graph precondition, Change result) { this.precondition = precondition; this.result = result;}

    public void DrawCard(Vector2 position) {
        Camera2D camera = new Camera2D(position, Vector2.Zero, 0, 1);
        BeginMode2D(camera);
        precondition.Draw();
        EndMode2D();
    }

    public (Node, Node)[]? findMapping(Graph graph) {
        return null;
    }

    public bool preconditionHolds(Graph graph, (Node, Node)[] mapping) {
        return false;
    }

    public void Apply(Graph graph, (Node, Node)[] mapping) {

    }


}