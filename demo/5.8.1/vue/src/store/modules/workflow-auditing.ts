import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';

interface WorkflowAuditingState extends ListState<any> {
    entity: any,
}

class WorkflowAuditingModule extends ListModule<WorkflowAuditingState, any, any>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: [],
        loading: false,
        entity: {},
    }
    actions = {
        async getAll(context: ActionContext<WorkflowAuditingState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/workFlowAudit/GetAll', { params: payload.data });
            context.state.loading = false;
            let page = reponse.data.result as PageResult<any>;
            context.state.totalCount = page.totalCount;
            context.state.list = page.items;
        },
        async create(context: ActionContext<WorkflowAuditingState, any>, payload: any) {
            await Ajax.post('/api/services/app/workFlowAudit/Create', payload.data);
        },
        async audit(context: ActionContext<WorkflowAuditingState, any>, payload: any) {
            await Ajax.post('/api/services/app/workFlowAudit/Audit', payload.data);
        },
        async getAuditRecords(context: ActionContext<WorkflowAuditingState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/workFlowAudit/GetAuditRecords?Id=' + payload.data.id);
            return reponse.data.result ;
        }
    };
    mutations = {
        setCurrentPage(state: WorkflowAuditingState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: WorkflowAuditingState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: WorkflowAuditingState, workflowAuditing: any) {
            state.entity = workflowAuditing;
        },
        setEntity(state: WorkflowAuditingState, workflowAuditing: any) {
            state.entity = workflowAuditing;
        }
    }
}
const workflowAuditingModule = new WorkflowAuditingModule();
export default workflowAuditingModule;