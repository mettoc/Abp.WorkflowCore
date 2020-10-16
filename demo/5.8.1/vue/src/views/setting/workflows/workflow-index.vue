<template>
  <div>
    <Card dis-hover>
      <div class="page-body">
        <Tabs v-model="tabValue">
          <TabPane label="首页" name="home">
            <workflow-definitions></workflow-definitions>
          </TabPane>
          <TabPane label="待处理" name="unaudit">
            <workflow-auditings :request="{auditedMark:false}" v-if="tabValue=='unaudit'"></workflow-auditings>
          </TabPane>
          <TabPane label="已处理" name="audited">
            <workflow-auditings :request="{auditedMark:true}" v-if="tabValue=='audited'"></workflow-auditings>
          </TabPane>
          <TabPane label="已发起" name="apply">
            <my-workflows v-if="tabValue=='apply'"></my-workflows>
          </TabPane>
        </Tabs>
      </div>
    </Card>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import PageRequest from "@/store/entities/page-request";
import WorkflowDefinitions from "./workflow-definition.vue";
import WorkflowAuditings from "./workflow-audit-list.vue";
import MyWorkflows from "./my-workflow.vue";
@Component({
  components: { WorkflowDefinitions, WorkflowAuditings, MyWorkflows },
})
export default class WorkflowIndexs extends AbpBase {
  tabValue = "home";
}
</script>
<style scoped>

</style>