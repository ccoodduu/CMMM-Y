//BASE
//  Doesn't contain step function.
//TICKED
//  Contains step function update order is random.
//TRACKED
//  Contains step function update order is by direction then distance from edge opposite cells facing.
public enum CellUpdateType_e { 
    BASE, TICKED, TRACKED
}