import Entity from "./entity"

export default class WorkflowInput extends Entity<string>{
    label: string;
    name: string;
    items: Array<any>;
    styles: Array<any>=[];
    rules: Array<any>=[];
}