<template>
  <div>
    <Card dis-hover>
      <div class="page-body">
        <Form ref="queryForm" :label-width="70" label-position="left" inline>
         <FormItem :label="L('Keyword')+':'" style="width:400px" >
                <Input v-model="pagerequest.title" :placeholder="L('Keyword')"></Input>
              </FormItem>
                <Button
                  icon="ios-search"
                  type="primary"
                  size="large"
                  @click="getpage"
                  class="toolbar-btn"
                >{{L('Find')}}</Button>
        </Form>
        <div class="margin-top-10">
          <Table
            :loading="loading"
            :columns="columns"
            :no-data-text="L('NoDatas')"
            border
            :data="list"
          ></Table>
          <Page
            show-sizer
            class-name="fengpage"
            :total="totalCount"
            class="margin-top-10"
            @on-change="pageChange"
            @on-page-size-change="pagesizeChange"
            :page-size="pageSize"
            :current="currentPage"
          ></Page>
        </div>
      </div>
    </Card>
    <workflow-details v-model="showWorkflowDetails" :workflow-id="currentSelectedRow.id"></workflow-details>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import PageRequest from "@/store/entities/page-request";
import WorkflowDetails from "./workflow-details.vue";
class PageWorkflowRequest extends PageRequest {
  title: string = "";
}

@Component({
  components: { WorkflowDetails },
})
export default class Workflows extends AbpBase {
  pagerequest: PageWorkflowRequest = new PageWorkflowRequest();
  showWorkflowDetails: boolean = false;
  currentSelectedRow = {};

  get list() {
    return this.$store.state.myWorkflow.list;
  }
  get loading() {
    return this.$store.state.myWorkflow.loading;
  }

  pageChange(page: number) {
    this.$store.commit("myWorkflow/setCurrentPage", page);
    this.getpage();
  }
  pagesizeChange(pagesize: number) {
    this.$store.commit("myWorkflow/setPageSize", pagesize);
    this.getpage();
  }
  async getpage() {
    this.pagerequest.maxResultCount = this.pageSize;
    this.pagerequest.skipCount = (this.currentPage - 1) * this.pageSize;

    await this.$store.dispatch({
      type: "myWorkflow/getAll",
      data: this.pagerequest,
    });
  }

  get pageSize() {
    return this.$store.state.myWorkflow.pageSize;
  }
  get totalCount() {
    return this.$store.state.myWorkflow.totalCount;
  }
  get currentPage() {
    return this.$store.state.myWorkflow.currentPage;
  }

  columns = [
    {
      type: "selection",
      width: 60,
      align: "center",
    },
    {
      title: this.L("提交时间"),
      key: "creationTime",
    },
    {
      title: this.L("流程名"),
      key: "title",
    },
     {
      title: this.L("当前任务节点"),
      key: "currentStepTitle",
    },
    {
      title: this.L("状态"),
      key: "status",
      render: (h: any, params: any) => {
        return h("div", [
          h(
            "Tag",
            {
              props: {
                color:
                  params.row.status == 0
                    ? "blue"
                    : params.row.status == 1
                    ? "orange"
                    : params.row.status == 2
                    ? "cyan"
                    : "red",
              },
            },
            params.row.status == 0
              ? "审核中"
              : params.row.status == 1
              ? "已暂停"
              : params.row.status == 2
              ? "已完成"
              : "已终止"
          ),
        ]);
      },
    },

    {
      title: this.L("Actions"),
      key: "Actions",
      width: 250,
      render: (h: any, params: any) => {
        return h("div", [
          h(
            "Button",
            {
              props: {
                type: "primary",
                size: "small",
              },
              on: {
                click: () => {
                  this.showDetails(params.row);
                },
              },
            },
            this.L("Details")
          ),
        ]);
      },
    },
  ];

  showDetails(row) {
    this.currentSelectedRow = row;
    this.showWorkflowDetails = true;
  }
  async created() {
    this.getpage();
  }
}
</script>