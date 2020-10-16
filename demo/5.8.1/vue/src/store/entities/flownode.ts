import Entity from "./entity"
import { ConditionFlowNode } from "./conditionflownode"


export default class FlowNode extends Entity<number>{
    key: string;
    title: string;
    icon: string;
    type: string;
    group: string;
    position:[number,number]
    endpointOptions: Array<any>;
    stepBody:any;
    parentNodes: Array<string>;
    nextNodes: Array<ConditionFlowNode>;
    public constructor(key?: string, title?: string, icon?: string, type?: string, group?: string, endpointOptions?: Array<any>) {
        super();
        this.key = key;
        this.title = title;
        this.icon = icon;
        this.type = type;
        this.group = group;
        this.endpointOptions = endpointOptions;
        this.parentNodes = [];
        this.nextNodes = [];
        this.position=[11,11];
        this.stepBody={};
    }
}