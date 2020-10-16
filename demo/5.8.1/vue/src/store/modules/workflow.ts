import Ajax from '../../lib/ajax';
import util from '../../lib/util'
import ListState from './list-state'
import FlowNode from '../entities/flownode'
import WorkflowDefinition from '../entities/workflow-definition'
import WorkflowInput from '../entities/workflow-input'

import PageResult from '@/store/entities/page-result';
import ListModule from './list-module'
import { Store, Module, ActionContext } from 'vuex'

interface WorkFlowState extends ListState<WorkflowDefinition> {
    nodes: Array<FlowNode>;
    jsPlumbOptions: any;
    stepBodys: Array<any>;
    endpointOptions: any;
    editWorkflow: WorkflowDefinition;
    rules: Array<any>;
    formComponents: Array<WorkflowInput>
}
class WorkFlowStore extends ListModule<WorkFlowState, any, WorkflowDefinition>{

    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<WorkflowDefinition>(),
        auditList: [],
        loading: false,
        editWorkflow: new WorkflowDefinition(),
        nodes: [
        ],
        formComponents: [
            {
                name: "",
                label: "单行文本",
                type: "text",
                styles: []
            },
            {
                name: "",
                label: "多行文本",
                type: "textarea",
                styles: []
            },
            {
                name: "",
                label: "复选框",
                type: "checkbox",
                items: [
                    { label: "item1", value: "item1" },
                    { label: "item2", value: "item2" },
                ],
                styles: []
            },
            {
                name: "",
                label: "单选框",
                type: "radio",
                items: [
                    { label: "item1", value: "item1" },
                    { label: "item2", value: "item2" },
                ],
                styles: []
            },
            {
                name: "",
                label: "文字",
                type: "paragraph",
                styles: []
            },
            {
                name: "",
                label: "下拉选择框",
                type: "select",
                items: [
                    { label: "item1", value: "item1" },
                    { label: "item2", value: "item2" },
                ],
                styles: []
            },
            {
                name: "",
                label: "日期选择器",
                type: "datepicker",
                styles: [{name:'width',value:"100%"}]
            },
            {
                name: "",
                label: "日期段选择器",
                type: "daterangepicker",
                styles: [{name:'width',value:"100%"}]
            },
            
            {
                name: "",
                label: "评分器",
                type: "rate",
                styles: []
            },
            {
                name: "",
                label: "开关选择器",
                type: "switch",
                styles: []
            },
            {
                name: "",
                label: "人民币输入框",
                type: "money",
                styles: [{name:'width',value:"100%"}]
            },
            {
                name: "",
                label: "数字输入框",
                type: "number",
                styles: [{name:'width',value:"100%"}]
            },
           
            {
                name: "",
                label: "图片上传",
                type: "uploadImages",
                styles: []
            },
            
        ],
        rules: [{
            name: "required",
            label: "必填",
            value: { required: true, message: "此项必填" },
        },
        {
            name: "number",
            label: "整数",
            value: { type: "string", message: "请输入数字", pattern: /^-?\d+$/ },
        },
        {
            name: "email",
            label: "邮箱",
            value: { type: 'email', message: "请输入正确的邮箱" },
        },
        {
            name: "phonenumber",
            label: "手机号",
            value: { type: "string", message: "请输入正确的手机号", pattern: /^1[3|4|5|7|8]d{9}$/ },
        },
    
    ],
        stepBodys: [],
        endpointOptions: {
            isSource: true,
            isTarget: true,
            connector: "Flowchart",
            maxConnections: 1,
            paintStyle: {
                fill: "#b0bac3",
                strokeWidth: 1,
            },
        },
        jsPlumbOptions: {
            Endpoint: [
                "Dot",
                {
                    radius: 4,
                    cssClass: "pink",
                    hoverClass: "pink",
                },
            ],
            PaintStyle: {
                stroke: "#b0bac3",
                strokeWidth: 2,
            },
            HoverPaintStyle: {
                stroke: "#b0bac3",
                strokeWidth: 4,
            },
            Container: "jsplumb-container",
            Connector: [
                "Flowchart",
                { gap: 10, cornerRadius: 5, alwaysRespectStubs: true },
            ],
            ConnectionOverlays: [
                [
                    "Arrow",
                    {
                        width: 10,
                        length: 10,
                        location: 0.8,
                    },
                ],
                [
                    "Label",
                    {
                        label: "",
                        cssClass: "",
                        id: "label",
                        labelStyle: {
                            color: "#515a6e",
                        },
                        events: {
                            click: function (labelOverlay, originalEvent) { },
                        },
                    },
                ],
            ],
        }
    }

    mutations = {

        setCurrentPage(state: WorkFlowState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: WorkFlowState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: WorkFlowState, workflow: WorkflowDefinition) {
            state.editWorkflow = workflow;
        },
    }
    actions = {
        async getAll(context: ActionContext<WorkFlowState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/workFlow/GetAll', { params: payload.data });
            context.state.loading = false;
            let page = reponse.data.result as PageResult<WorkflowDefinition>;
            context.state.totalCount = page.totalCount;
            context.state.list = page.items;
        },
        async update(context: ActionContext<WorkFlowState, any>, payload: any) {
            await Ajax.put('/api/services/app/workFlow/Update', payload.data);
        },
        async delete(context: ActionContext<WorkFlowState, any>, payload: any) {
            await Ajax.delete('/api/services/app/workFlow/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<WorkFlowState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/workFlow/Get?Id=' + payload.data.id);
            return reponse.data.result as WorkflowDefinition;
        },
        async getAllGroup(context: ActionContext<WorkFlowState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/workFlow/GetAllGroup');
            return reponse.data.result;
        },
        async getAllWithGroup(context: ActionContext<WorkFlowState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/workFlow/GetAllWithGroup');
            return reponse.data.result;
        },

        async getDetails(context: ActionContext<WorkFlowState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/workFlow/GetDetails?Id=' + payload.data.id);
            return reponse.data.result;
        },
        async checked(content: ActionContext<WorkFlowState, any>, payload: any) {
            let user = payload.data;
        },
        async start(content: ActionContext<WorkFlowState, any>, payload: any) {
            await Ajax.post('/api/services/app/workFlow/start', payload.data);
        },
        async create(content: ActionContext<WorkFlowState, any>, payload: any) {
            await Ajax.post('/api/services/app/workFlow/create', payload.data);
        },

        async getStepBodys(context: ActionContext<WorkFlowState, any>) {
            let reponse = await Ajax.get('/api/services/app/workFlow/GetAllStepBodys');
            context.state.stepBodys = reponse.data.result;
            return reponse.data.result
        },
        async init(content: ActionContext<WorkFlowState, any>) {
            await content.dispatch({
                type: "getStepBodys"
            })
        }
    }
}
const workFlow = new WorkFlowStore();

workFlow.state.nodes.push(new FlowNode("start",
    "流程开始",
    "md-play",
    "success",
    "1",
    [
        {
            anchor: "Bottom",
            maxConnections: -1,
        },
    ]));
workFlow.state.nodes.push(new FlowNode(
    "end",
    "结束",
    "md-square",
    "error",
    "1",
    [
        {
            anchor: "Top",
            maxConnections: -1,
        },
    ]))
workFlow.state.nodes.push(new FlowNode(
    "step",
    "任务节点",
    "md-settings",
    "primary",
    "2",
    [
        {
            anchor: "Top",
            maxConnections: -1,
        },
        {
            anchor: "Bottom",
            maxConnections: -1,
        }
    ]))
export default workFlow;