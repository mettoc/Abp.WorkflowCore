<template>
  <div>
    <!-- <Card dis-hover> -->
      <div class="page-body">
        <Button to="create" icon="md-add" type="primary" size="large">{{L('Add')}}</Button>
        <div class="margin-top-10">
          <Collapse v-model="collapseValues">
            <Panel :name="key" v-for="(item,key) in workflows" :key="key">
              {{key}}
              <div slot="content" class="margin-top-10">
                <Poptip trigger="click" v-for="(w,key) in item" :key="key" placement="top">
                  <div slot="content">
                    <Button
                      size="small"
                      type="success"
                      icon="md-play"
                      :title="L('发起流程')"
                      @click="showStartWorkflowBox(w)"
                    >{{L('发起')}}</Button>
                    
                    <Button
                      size="small"
                      class="toolbar-btn"
                      type="primary"
                      :to="'/main/edit/' + w.id"
                      icon="md-create"
                    >{{L("编辑")}}</Button>
                    <Button
                      size="small"
                      class="toolbar-btn"
                      icon="md-trash"
                      type="error"
                      @click="Delete(w)"
                    >{{L("删除")}}</Button>
                  </div>
                  <Card class="w-item" :style="{'width':'220px'}" style="width:220px">
                    <Icon
                      :type="w.icon||'md-git-merge'"
                      :style="{'background-color':w.color||'#2b85e4'}"
                      color="#fff"
                      size="32"
                    ></Icon>
                   <strong> {{w.title}}</strong>
                  </Card>
                </Poptip>
              </div>
            </Panel>
          </Collapse>
        </div>
      </div>
    <!-- </Card> -->

    <start-workflow v-model="showStartWorkflow" :workflow="currentSelectedRow"></start-workflow>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import PageRequest from "@/store/entities/page-request";
import StartWorkflow from "./start-workflow.vue";

class PageWorkflowRequest extends PageRequest {
  title: string = "";
}

@Component({
  components: { StartWorkflow },
})
export default class Workflows extends AbpBase {
  pagerequest: PageWorkflowRequest = new PageWorkflowRequest();
  showStartWorkflow: boolean = false;
  currentSelectedRow = {};
  collapseValues = [];
  workflows = [];
  async getpage() {
    this.workflows = await this.$store.dispatch({
      type: "workflow/getAllWithGroup",
      data: this.pagerequest,
    });
  }
  async showStartWorkflowBox(item) {
    this.currentSelectedRow = JSON.parse(JSON.stringify(item));
    this.showStartWorkflow = true;
  }

  async Delete(item) {
    this.$Modal.confirm({
      title: this.L("Tips"),
      content: this.L("DeleteWorkflowsConfirm"),
      okText: this.L("Yes"),
      cancelText: this.L("No"),
      onOk: async () => {
        await this.$store.dispatch({
          type: "workflow/delete",
          data: item,
        });
        await this.getpage();
      },
    });
  }

  async created() {
    await this.getpage();
    for (let item in this.workflows) {
      this.collapseValues.push(item);
    }
  }
}
</script>
<style scoped>
.w-item {
  display: inline-block;
  margin-left: 20px;
  cursor: pointer;
}
.w-item i {
  border-radius: 5px;
  padding: 5px;
}
</style>