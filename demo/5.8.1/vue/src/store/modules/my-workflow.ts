import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';

interface MyWorkflowState extends ListState<any> {
    entity: any,
}

class MyWorkflowModule extends ListModule<MyWorkflowState, any, any>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: [],
        loading: false,
        entity: {},
    }
    actions = {
        async getAll(context: ActionContext<MyWorkflowState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/workflow/GetMyWorkflow', { params: payload.data });
            context.state.loading = false;
            let page = reponse.data.result as PageResult<any>;
            context.state.totalCount = page.totalCount;
            context.state.list = page.items;
        },
    };
    mutations = {
        setCurrentPage(state: MyWorkflowState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: MyWorkflowState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: MyWorkflowState, myWorkflow: any) {
            state.entity = myWorkflow;
        },
        setEntity(state: MyWorkflowState, myWorkflow: any) {
            state.entity = myWorkflow;
        }
    }
}
const myWorkflowModule = new MyWorkflowModule();
export default myWorkflowModule;