<template>
  <div>
    <Drawer
      :title="L('Details')"
      :draggable="true"
      :width="500"
      :closable="true"
      v-model="value"
      @on-visible-change="visibleChange"
    >
      <div class="wf-detail">
        <Card dis-hover>
          <Row>
            <Col span="24">
              <h3>{{entity.userName+" "+L('Submited')+L(entity.title)}}</h3>
              <p>
                <Tag color="primary" v-if="entity.status==0">{{L("审核中")}}</Tag>
                <Tag color="error" v-if="entity.status==3">{{L("已终止")}}</Tag>
                <Tag color="success" v-if="entity.status==2">{{L("已完成")}}</Tag>
              </p>
            </Col>
          </Row>
          <Divider dashed size="small"></Divider>
          <Row>
            <Col span="24">
              <Form ref="flowForm" label-position="top" :model="entity.data">
             <Row :gutter="15" v-for="(row,rowkey) in entity.inputs" :key="rowkey">
                  <Col
                    v-for="(column,key) in row"
                    :key="key"
                    :span="24/row.length"
                  >
                    <workflow-form-item
                      v-model="entity.data[element.name]"
                      :element="column[key]"
                      v-for="(element,key) in column"
                      :key="element.id"
                      :disabled="true"
                    ></workflow-form-item>
                  </Col>
                </Row>
              </Form>
            </Col>
          </Row>
        </Card>
        <Card dis-hover class="margin-top-10">
          <Timeline>
            <TimelineItem v-for="(item,key ) in entity.executionRecords" :key="key">
              <p class="time">{{item.stepTitle}}</p>
              <div class="content">
                <Timeline>
                  <TimelineItem
                    v-for="(audit,key) in auditEntity.auditRecords[item.executionPointerId]"
                    :key="key"
                  >
                    <Avatar size="small" slot="dot" :src="audit.userHeadPhoto" v-if="audit.userHeadPhoto"></Avatar>
                     <Avatar size="small" slot="dot" icon="ios-person" v-else></Avatar>
                    <p class="time">
                      {{audit.userIdentityName}}
                      <Tag
                        class="small"
                        :color="audit.status==0?'blue':audit.status==1?'cyan':'red'"
                      >{{audit.status==0?'待审核':audit.status==1?'已同意':'已拒绝'}}</Tag>
                      <span>{{audit.auditTime}}</span>
                    </p>
                    <div class="content">
                      <div v-if="audit.status==0 && auditEntity.needAudit && audit.userId==userId">
                        <Input type="textarea" v-model="remark" placeholder="Enter Remark..." />
                        <Divider size="small" style="margin:9px 0px"></Divider>
                        <Button
                          type="success"
                          size="small"
                          style="margin-right: 8px"
                          @click="SubmitAudit(item.executionPointerId,true)"
                        >{{L("同意")}}</Button>
                        <Button
                          type="error"
                          size="small"
                          @click="SubmitAudit(item.executionPointerId,false)"
                        >{{L("拒绝")}}</Button>
                      </div>
                      <p v-if="audit.status!=0 && audit.remark">备注：{{audit.remark}}</p>
                    </div>
                  </TimelineItem>
                </Timeline>
              </div>
            </TimelineItem>
          </Timeline>
        </Card>
        <!-- <div class="demo-drawer-footer">
          <Button type="Default" style="margin-right: 8px" @click="value3 = false">{{L("Cancel")}}</Button>
        </div>-->
      </div>
    </Drawer>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "../../../lib/util";
import AbpBase from "../../../lib/abpbase";
import WorkflowFormItem from "./workflow-form-item.vue";
@Component({ components: { WorkflowFormItem } })
export default class WorkflowDetails extends AbpBase {
  @Prop({ type: Boolean, default: false }) value: boolean;
  @Prop({ type: String, default: null }) workflowId: String;
  entity: any = {};
  auditEntity: any = {};
  remark = "";
  async visibleChange(value: boolean) {
    if (!value) {
      this.$emit("input", value);
      return;
    }
    await this.init();
  }

  get userId(){
    return this.$store.state.session.user.id;
  }
  async init() {
    this.remark=null;
    if (this.workflowId) {
      this.entity = await this.$store.dispatch({
        type: "workflow/getDetails",
        data: { id: this.workflowId },
      });
      this.auditEntity = await this.$store.dispatch({
        type: "workflowAuditing/getAuditRecords",
        data: { id: this.workflowId },
      });
    }
  }
  async SubmitAudit(pointid: any, pass: boolean) {
    if (!pass && this.remark.length <= 0) {
      this.$Message.error(this.L("请填写拒绝理由！"));
      return;
    }
    await this.$store.dispatch({
      type: "workflowAuditing/audit",
      data: { executionPointerId: pointid, pass: pass, remark: this.remark },
    });
    await this.init();
  }
}
</script>
<style scoped>
.wf-detail {
  /* background: #f0f2f5; */
  height: 100%;
}
.wf-detail .ivu-card {
  border-radius: 0px;
  border: 0px;
}
.demo-drawer-footer {
  width: 100%;
  position: absolute;
  bottom: 0;
  left: 0;
  border-top: 1px solid #e8e8e8;
  padding: 10px 16px;
  text-align: right;
  background: #fff;
}

.ivu-timeline .time {
  position: relative;
}
.ivu-timeline .content {
  margin: 10px 5px;
}

.ivu-timeline .time .small {
  height: 17px;
  line-height: 17px;
  margin-left: 10px;
}

.ivu-timeline .time span {
  position: absolute;
  right: 0px;
  color: #bbb;
}
</style>

