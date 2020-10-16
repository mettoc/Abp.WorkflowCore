import Entity from "./entity"
import FlowNode from "./FlowNode"



export class Condition {
    field: string;
    operator: string;
    value: string
}
export  class ConditionFlowNode extends Entity<number>{

    label: string
    nodeId: string 
    conditions?: Array<Condition>
    public constructor(nodeid: string ) {
        super();
        this.nodeId = nodeid;
        this.label = "";
        this.conditions = [];
    }
}




