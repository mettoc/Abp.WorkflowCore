declare global {
    interface RouterMeta {
        title: string;
    }
    interface Router {
        path: string;
        name: string;
        icon?: string;
        permission?: string;
        meta?: RouterMeta;
        component: any;
        children?: Array<Router>;
    }
    interface System {
        import(request: string): Promise<any>
    }
    var System: System
}
import login from '../views/login.vue'
import home from '../views/home/home.vue'
import main from '../views/main.vue'

export const locking = {
    path: '/locking',
    name: 'locking',
    component: () => import('../components/lockscreen/components/locking-page.vue')
};
export const loginRouter: Router = {
    path: '/',
    name: 'login',
    meta: {
        title: 'LogIn'
    },
    component: () => import('../views/login.vue')
};
export const otherRouters: Router = {
    path: '/main',
    name: 'main',
    permission: '',
    meta: { title: 'ManageMenu' },
    component: main,
    children: [
        { path: 'home', meta: { title: 'HomePage' }, name: 'home', component: () => import('../views/home/home.vue') },
        { path: 'edit/:id',  icon: "", meta: { title: 'Edit WorkFlow' }, name: 'editWorkflow', component: () => import('../views/setting/workflows/edit-workflow.vue') },
    ]
}
export const appRouters: Array<Router> = [{
    path: '/setting',
    name: 'setting',
    permission: '',
    meta: { title: 'ManageMenu' },
    icon: '&#xe68a;',
    component: main,
    children: [
       
        { path: 'user', permission: 'Pages.Users', meta: { title: 'Users' }, name: 'user', component: () => import('../views/setting/user/user.vue') },
        { path: 'role', permission: 'Pages.Roles', meta: { title: 'Roles' }, name: 'role', component: () => import('../views/setting/role/role.vue') },
        { path: 'tenant', permission: 'Pages.Tenants', meta: { title: 'Tenants' }, name: 'tenant', component: () => import('../views/setting/tenant/tenant.vue') }
    ]
},
{
    path: '/workflow',
    name: 'workflow',
    permission: '',
    meta: { title: 'WorkflowManager' },
    icon: '&#xe68a;',
    component: main,
    children: [
        { path: 'index', icon: "", meta: { title: 'WorkFlow List' }, name: 'workflowlist', component: () => import('../views/setting/workflows/workflow-index.vue') },
        { path: 'create',  icon: "", meta: { title: 'Create WorkFlow' }, name: 'createWorkflow', component: () => import('../views/setting/workflows/create-workflow.vue') },
        
    ]
},
]
export const routers = [
    loginRouter,
    locking,
    ...appRouters,
    otherRouters
];
