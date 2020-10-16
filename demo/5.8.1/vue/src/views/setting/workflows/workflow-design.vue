<template>
  <div class="demo-split">
    <Split v-model="split">
      <div slot="left" class="demo-split-pane">
        <Collapse v-model="panel">
          <Panel name="1">
            {{L('活动节点')}}
            <div slot="content">
              <Button
                :type="item.type"
                v-for="(item,key) in sourceNodes"
                :key="key"
                :icon="item.icon"
                :style="{'margin':'2px'}"
                @dblclick.native="addNodeByType(item.type)"
              >{{L(item.title)}}</Button>
            </div>
          </Panel>
        </Collapse>
      </div>
      <div slot="right" class="demo-split-pane right" style="padding-left:6px;">
        <Split v-model="split1">
          <div slot="left" class="demo-split-pane">
            <div id="jsplumb-container" class="jsplumb-container">
              <Button
                :type="item.type"
                v-for="(item,key) in value"
                :key="key"
                :icon="item.icon"
                :id="item.key"
                class="wf-btn"
                @mousedown.native="setNode(item)"
              >{{L(item.title)}}</Button>
            </div>
          </div>
          <div slot="right" class="demo-split-pane" style="padding-left:6px">
            <Tabs v-model="tabValue" style="height:100%">
              <TabPane :label="L('连线')" name="connection" :disabled="!isclickLine">
                <Form
                  ref="connectionForm"
                  label-position="top"
                  :model="conditionNode"
                  style="padding:5px;"
                >
                  <FormItem :label="L('连线标题')">
                    <Input v-model="conditionNode.label" @on-keyup="revalidateConnection"></Input>
                  </FormItem>
                  <FormItem :label="L('条件')">
                    <Row
                      :gutter="5"
                      v-for="(item, index) in conditionNode.conditions"
                      :key="index"
                      class="margin-top-10"
                    >
                      <Col span="6">
                        <Select v-model="item.field">
                          <Option
                            v-for="item in workflowInputs"
                            :value="item.name"
                            :key="item.name"
                          >{{ item.name }}</Option>
                        </Select>
                      </Col>
                      <Col span="6">
                        <Select v-model="item.operator">
                          <Option value=">">{{L('大于')}}</Option>
                          <Option value=">=">{{L('大于等于')}}</Option>
                          <Option value="<">{{L('小于')}}</Option>
                          <Option value="<=">{{L('小于等于')}}</Option>
                          <Option value="!=">{{L('不等于')}}</Option>
                          <Option value="==">{{L('等于')}}</Option>
                        </Select>
                      </Col>
                      <Col span="6">
                        <Input v-model="item.value" :minlength="1"></Input>
                      </Col>
                      <Col span="6">
                        <Button @click="removeCondition(index)">{{L("Delete")}}</Button>
                      </Col>
                    </Row>
                  </FormItem>
                  <FormItem>
                    <Row>
                      <Col span="24">
                        <Button type="dashed" long @click="addCondition" icon="md-add">{{L("添加条件")}}</Button>
                      </Col>
                    </Row>
                    <Row class="margin-top-10">
                      <Col span="24">
                        <Button
                          type="error"
                          ghost
                          long
                          @click="removeConnection"
                          icon="md-trash"
                        >{{L("删除连线")}}</Button>
                      </Col>
                    </Row>
                  </FormItem>
                </Form>
              </TabPane>
              <TabPane :label="L('节点')" name="node" :disabled="isclickLine">
                <Form ref="nodeForm" label-position="top" :model="currentNode" style="padding:5px;">
                  <FormItem :label="L('标识')">
                    <Input v-model="currentNode.key" disabled></Input>
                  </FormItem>
                  <FormItem :label="L('标题')">
                    <Input v-model="currentNode.title" :minlength="1" @on-keyup="revalidate"></Input>
                  </FormItem>
                  <FormItem :label="L('执行操作')">
                    <Select v-model="tempStepBodyName" @on-change="onStepBodyChange">
                      <Option
                        v-for="(item,key) in stepBodys"
                        :value="item.name"
                        :key="key"
                      >{{ item.displayName }}</Option>
                    </Select>
                  </FormItem>

                  <FormItem
                    :label="L(item.displayName)+'：'"
                    v-for="(item,key) in currentNode.stepBody.inputs"
                    :key="key"
                  >
                    <Input
                      type="text"
                      v-if="item.inputType.name=='SINGLE_LINE_STRING'&& tempStepBodyName"
                      v-model="item.value"
                    ></Input>
                    <Checkbox
                      v-model="item.value"
                      v-if="item.inputType.name=='CHECKBOX'&& tempStepBodyName"
                    >{{L(item.displayName)}}</Checkbox>
                    <select-user
                      v-model="item.value"
                      v-if="item.inputType.name=='SELECT_USERS' && tempStepBodyName"
                    ></select-user>

                    <select-role
                      v-model="item.value"
                      v-if="item.inputType.name=='SELECT_ROLES' && tempStepBodyName"
                    ></select-role>
                  </FormItem>

                  <Row class="margin-top-10">
                    <Col span="24">
                      <Button
                        type="error"
                        ghost
                        long
                        @click="removeNode"
                        icon="md-trash"
                      >{{L("Delete Node")}}</Button>
                    </Col>
                  </Row>
                </Form>
              </TabPane>
            </Tabs>
          </div>
        </Split>
      </div>
    </Split>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import {
  ConditionFlowNode,
  Condition,
} from "@/store/entities/conditionflownode";
import FlowNode from "@/store/entities/flownode";
import {
  jsPlumb,
  jsPlumbInstance,
  EndpointOptions,
  ConnectorSpec,
  EndpointSpec,
  DragEventCallbackOptions,
} from "jsplumb";
import SelectUser from "@/components/select-users.vue";
import SelectRole from "@/components/select-roles.vue";
import draggable from "vuedraggable";
@Component({
  components: { SelectUser, SelectRole, draggable },
})
export default class WorkflowDesign extends AbpBase {
  @Prop({ type: Array, default: [] }) value: Array<FlowNode>;
  @Prop({ type: Array, default: [] }) inputs: Array<any>;

  plumbIns: jsPlumbInstance = null;
  split = 0.15;
  split1 = 0.8;
  panel = ["1", "2"];
  isclickLine = false;
  currentNode: FlowNode = new FlowNode();
  conditionNode: ConditionFlowNode = new ConditionFlowNode(null);
  tabValue = "node";
  currentConnection = null;
  currentStepIndex = 0;
  tempStepBodyName = "";

  get sourceNodes() {
    return this.$store.state.workflow.nodes;
  }
  get workflowInputs() {
    let arr = [];
    this.inputs.forEach((element) => {
      element.forEach((obj) => {
        arr.push(...obj);
      });
    });
    return arr;
  }

  @Watch("value", { deep: true })
  updateData1() {
    this.$emit("input", this.value);
  }
  @Watch("currentNode", { deep: true })
  updateData(value) {
    this.$emit("input", this.value);
  }

  btns(group) {
    return this.sourceNodes.filter((u) => u.group == group);
  }

  get stepBodys() {
    return this.$store.state.workflow.stepBodys;
  }

  //重画节点
  revalidate() {
    this.$nextTick(() => {
      this.plumbIns.revalidate(this.currentNode.key);
    });
  }
  setNode(step) {
    this.tabValue = "node";
    this.isclickLine = false;
    this.currentNode = step;
    this.tempStepBodyName = null;
    if (this.currentNode.stepBody && this.currentNode.stepBody.name) {
      let sourceBody = JSON.parse(
        JSON.stringify(
          this.stepBodys.filter(
            (u) => u.name == this.currentNode.stepBody.name
          )[0]
        )
      );
      this.currentNode.stepBody = {
        ...Util.extend(true, sourceBody, this.currentNode.stepBody),
      };
      setTimeout(() => {
        this.tempStepBodyName = this.currentNode.stepBody.name;
      }, 10);
    }
  }
  getNode(key) {
    return this.value.filter((u) => u.key == key)[0];
  }
  onStepBodyChange(value) {
    this.currentNode.stepBody = JSON.parse(
      JSON.stringify(this.stepBodys.filter((u) => u.name == value)[0])
    );
  }

  removeNode() {
    let i = 0;
    for (let index = 0; index < this.value.length; index++) {
      if (this.value[index].key == this.currentNode.key) {
        break;
      }
      i++;
    }

    this.value.forEach((node) => {
      node.nextNodes = node.nextNodes.filter(
        (u) => u.nodeId != this.currentNode.key
      );
      node.parentNodes = node.parentNodes.filter(
        (u) => u != this.currentNode.key
      );
    });
    this.plumbIns.remove(this.currentNode.key);
    this.value.splice(i, 1);
  }

  createNodeByType(type: string, key?: string) {
    let node = JSON.parse(
      JSON.stringify(this.sourceNodes.filter((u) => u.type == type)[0])
    );
    node.key =
      key !== undefined
        ? key
        : node.key + "_" + Date.now() + Math.random().toString(36).substr(2);
    if (node.endpointOptions !== null) {
      node.endpointOptions.forEach((option) => {
        option.uuid = node.key + option.anchor;
      });
    }
    return node;
  }

  addNodeByType(type: string) {
    if (
      type == "success" &&
      this.value.filter((i) => i.type == type).length > 0
    ) {
      this.$Message.error("一个流程只能有一个开始节点");
      return;
    }
    let node = this.createNodeByType(type);
    this.value.push(node);
    this.$nextTick(() => {
      this.addNode(node);
    });
  }

  //添加节点
  addNode(node: FlowNode) {
    let _this = this;
    this.plumbIns.draggable(node.key, {
      containment: "jsplumb-container",
      drag: (params: DragEventCallbackOptions) => {
        _this.currentNode.position = params.pos;
      },
    });
    let domNode = document.getElementById(node.key);
    domNode.style.left = node.position[0] + "px";
    domNode.style.top = node.position[1] + "px";
    if (node.endpointOptions !== null) {
      node.endpointOptions.forEach((option) => {
        this.plumbIns.addEndpoint(
          node.key,
          option,
          this.$store.state.workflow.endpointOptions
        );
      });
    }
  }
  addCondition() {
    this.conditionNode.conditions.push(new Condition());
  }
  removeCondition(index) {
    this.conditionNode.conditions.splice(index, 1);
  }

  //连接事件
  onConnection(info: any) {
    let sourse: FlowNode = this.value.filter((u) => u.key == info.sourceId)[0];
    let target: FlowNode = this.value.filter((u) => u.key == info.targetId)[0];
    if (target.parentNodes.filter((u) => u == sourse.key).length <= 0) {
      target.parentNodes.push(sourse.key);
    }
    if (sourse.nextNodes.filter((u) => u.nodeId == target.key).length <= 0) {
      let c = new ConditionFlowNode(target.key);
      sourse.nextNodes.push(c);
    }
  }
  removeConnection() {
    this.$Modal.confirm({
      title: this.L("Tips"),
      content: this.L("DeleteConnectionConfirm"),
      okText: this.L("Yes"),
      cancelText: this.L("No"),
      onOk: async () => {
        let source = this.getNode(this.currentConnection.sourceId);
        let target = this.getNode(this.currentConnection.targetId);
        source.nextNodes = source.nextNodes.filter(
          (u) => u.nodeId != this.currentConnection.targetId
        );
        target.parentNodes = source.parentNodes.filter(
          (u) => u != this.currentConnection.sourceId
        );
        this.plumbIns.deleteConnection(this.currentConnection);
      },
    });
  }
  revalidateConnection() {
    if (this.currentConnection !== null) {
      this.currentConnection
        .getOverlay("label")
        .setLabel(this.conditionNode.label);
    }
  }

  onClickConnection(connection: any, originalEvent: any) {
    let source: FlowNode = this.value.filter(
      (u) => u.key == connection.sourceId
    )[0];
    this.conditionNode = source.nextNodes.filter(
      (u) => u.nodeId == connection.targetId
    )[0];
    this.currentConnection = connection;
    this.isclickLine = true;
    this.tabValue = "connection";
  }
  async initWorkflow() {
    this.value = this.value.map((node) => {
      let snode = this.createNodeByType(node.type, node.key);
      return Util.extend(snode, node);
    });
    await this.$nextTick(() => {
      this.value.forEach((node) => {
        this.addNode(node);
      });
      let createConnect = (node) => {
        let i =
            node.parentNodes.length > 0 && node.endpointOptions.length > 1
              ? 1
              : 0,
          j = 0,
          nodeEndpoints = node.endpointOptions;

        node.nextNodes.forEach((nnode) => {
          let nnodeEndpoints = this.getNode(nnode.nodeId).endpointOptions;
          this.plumbIns.connect({
            uuids: [nodeEndpoints[nodeEndpoints.length-1].uuid, nnodeEndpoints[0].uuid],
            label: nnode.label,
            endpoint: this.$store.state.workflow.endpointOptions,
          });
          i++;
          i = i >= nodeEndpoints.length ? 0 : i;
        });
      };
      this.value.forEach((node) => {
        createConnect(node);
      });
    });
  }

  async mounted() {
    this.plumbIns = jsPlumb.getInstance(
      this.$store.state.workflow.jsPlumbOptions
    );
    await this.$store.dispatch({
      type: "workflow/init",
    });
    this.$nextTick(() => {
      this.plumbIns.ready(() => {
        this.plumbIns.setContainer("jsplumb-container");
        this.plumbIns.bind("connection", this.onConnection);
        this.plumbIns.bind("click", this.onClickConnection);
        if (this.value.length > 0) {
          this.initWorkflow();
        }
      });
    });
  }
}
</script>
<style scoped>
.jsplumb-container {
  height: 100%;
  margin-left: 6px;
  position: relative;
  padding: 10px;
}

.demo-split {
  height: calc(100% - 30px);
  min-height: 500px;
  border: 1px solid #dcdee2;
  background: #fff;
  width: 100%;
  position: absolute;
}
.ivu-steps {
  background: #fff;
  height: 30px;
}
.demo-tool .ivu-btn {
  margin-left: 10px;
}

.demo-split-pane {
  height: 100%;
}

.item {
  position: absolute;
  height: 50px;
  width: 50px;
  border: 1px solid #bbb;
}
.wf-btn {
  position: absolute !important;
}
.ivu-collapse {
  border-radius: 0px;
  border: 0px;
}
</style>