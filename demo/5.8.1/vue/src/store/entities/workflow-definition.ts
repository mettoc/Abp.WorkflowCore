import Entity from "./entity"
import FlowNode from "./flownode";


export default class WorkflowDefinition extends Entity<string>{
    title: string;
    version:number=1;
    description: string;
    icon:string;
    color:string='';
    group:string;
    inputs: Array<any>;
    nodes: Array<FlowNode>;
}
