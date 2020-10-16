<template>
  <div class="form-split">
    <Split v-model="split">
      <div slot="left" class="form-split-pane">
        <Collapse v-model="panel">
          <Panel name="1">
            {{L('表单组件')}}
            <div slot="content">
              <Form label-position="top" class="components-form">
                <draggable
                  v-model="formComponents"
                  :clone="clone"
                  :group="{ name: 'input', pull: 'clone', put: false }"
                >
                  <Button
                    type="dashed"
                    v-for="(element,key) in formComponents"
                    :key="key"
                     style="margin:5px"
                  >{{element.label}}</Button>
                </draggable>
              </Form>
            </div>
          </Panel>
        </Collapse>
      </div>
      <div slot="right" class="form-split-pane right" style="padding-left:6px;">
        <Split v-model="split1">
          <div slot="left" class="form-split-pane center">
            <Form label-position="top">
              
              <draggable v-model="value" group="row">
                <Row
                  :gutter="5"
                  v-for="(row,rowkey) in value"
                  :key="rowkey"
                  @mouseover.native="currentRow=value[rowkey]"
                  @mouseout.native="currentRow=null"
                >
                  <div class="form-tool" v-if="currentRow==row">
                    <Icon type="md-trash" size="15" @click.native="removeRow(rowkey)"></Icon>
                  </div>
                  <Col v-for="(column,colkey) in row" :key="colkey" :span="24/row.length">
                    <p class="dragg-info" v-if="column.length<=0">{{L('请将组件拖至此处')}}</p>
                    <draggable v-model="value[rowkey][colkey]" group="input">
                      <div
                        :class="{'draggable-item':true,'active':element.id==currentInput.id}"
                        v-for="(element,key) in column"
                        :key="element.id"
                        @click="setAttribute(element)"
                      >
                        <workflow-form-item :element="column[key]" v-model="element.value"></workflow-form-item>
                        <div class="form-tool" v-if="element.id==currentInput.id">
                          <Icon type="md-trash" size="15" @click.native="removeComponent"></Icon>
                        </div>
                      </div>
                    </draggable>
                  </Col>
                </Row>
              </draggable>
              <div class="form-row-btn">
                <Poptip placement="bottom">
                  <div slot="content">
                    <ButtonGroup vertical>
                      <Button
                        @click="oncolumnbtnClick(1)"
                      >{{L('一行一列')}}</Button>
                            <Button
                        @click="oncolumnbtnClick(3)"
                      >{{L('一行三列')}}</Button>
                    </ButtonGroup>
                    <ButtonGroup vertical style="margin-left:10px">
                       <Button
                        @click="oncolumnbtnClick(2)"
                      >{{L('一行两列')}}</Button>
               
                      <Button
                        @click="oncolumnbtnClick(4)"
                      >{{L('一行四列')}}</Button>
                    </ButtonGroup>
                  </div>
                  <Button type="text" icon="md-add-circle">添加行</Button>
                </Poptip>
              </div>
            </Form>
          </div>
          <div slot="right" class="form-split-pane right">
            <Form
              ref="attrForm"
              label-position="top"
              :model="currentInput"
              v-if="currentInput.id"
              class="attr-form"
            >
              <FormItem :label="L('标识')">
                <Input v-model="currentInput.id" disabled></Input>
              </FormItem>
              <FormItem :label="L('标签')+'：'">
                <Input v-model="currentInput.label"></Input>
              </FormItem>
              <FormItem :label="L('字段名')+'：'">
                <Input v-model="currentInput.name"></Input>
              </FormItem>
              
              <FormItem :label="L('长度')+'：'">
                <Row :gutter="15">
                  <Col span="11">
                    <Input type="number" v-model="currentInput.minlength" :placeholder="L('最小长度')"></Input>
                  </Col>
                  <Col span="2">-</Col>
                  <Col span="11">
                    <Input type="number" v-model="currentInput.maxlength" :placeholder="L('最大长度')"></Input>
                  </Col>
                </Row>
              </FormItem>
              <FormItem>
                <div slot="label">
                  {{L('样式')+'：'}}
                  <Button type="dashed" class="btn-right" @click="addStyle" icon="md-add"></Button>
                </div>
                <Row :gutter="5" v-for="(item, index) in currentInput.styles" :key="index">
                  <Col span="8">
                    <Input size="small" v-model="item.name"></Input>
                  </Col>
                  <Col span="2">：</Col>
                  <Col span="10">
                    <Input size="small" v-model="item.value"></Input>
                  </Col>
                  <Col span="2">
                    <Button size="small" @click="removeStyle(index)" icon="md-trash" type="error"></Button>
                  </Col>
                </Row>
              </FormItem>

              <FormItem :label="L('选项')+'：'" v-if="currentInput.items">
                <Row :gutter="5" v-for="(item, index) in currentInput.items" :key="index">
                  <Col span="8">
                    <Input size="small" :placeholder="L('标签')" v-model="item.label"></Input>
                  </Col>
                  <Col span="8">
                    <Input size="small" :placeholder="L('值')" v-model="item.value"></Input>
                  </Col>
                  <Col span="8">
                    <Button size="small" @click="removeItem(index)" icon="md-trash"></Button>
                  </Col>
                </Row>
                <Row class="margin-top-10">
                  <Col>
                    <Button type="dashed" long @click="addItem" icon="md-add">{{L("Add Item")}}</Button>
                  </Col>
                </Row>
              </FormItem>
              <FormItem :label="L('数据验证')+'：'">
                <CheckboxGroup v-model="currentInput.rules">
                  <Checkbox :label="i.name" v-for="(i,key) in formRules" :key="key">{{L(i.label)}}</Checkbox>
                </CheckboxGroup>
              </FormItem>
          
            </Form>
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
import draggable from "vuedraggable";
import WorkflowInput from "@/store/entities/workflow-input";
import WorkflowFormItem from "./workflow-form-item.vue";

@Component({
  components: { draggable, WorkflowFormItem },
})
export default class FormDesign extends AbpBase {
  @Prop({ type: Array, default: [] }) value: Array<any>;

  split = 0.15;
  split1 = 0.8;
  panel = ["1", "2"];
  componentCount = 0;
  currentRow = null;
  oncolumnbtnClick(col) {
    this.value.push([]);
    for (let i = 0; i < col; i++) {
      this.value[this.value.length - 1].push([]);
    }
  }

  removeRow(index) {
    this.value.splice(index, 1);
  }

  currentInput: WorkflowInput = new WorkflowInput();
  setAttribute(item) {
    this.currentInput = item;
  }
  removeItem(index) {
    this.currentInput.items.splice(index, 1);
  }
  addItem() {
    this.currentInput.items.push({ label: "", value: "" });
  }

  removeStyle(index) {
    this.currentInput.styles.splice(index, 1);
  }
  addStyle() {
    if (!this.currentInput.styles) {
      this.currentInput.styles = [];
    }
    this.currentInput.styles.push({ name: "", value: "" });
  }

  clone(item) {
    let i = { ...item };
    i.id = Date.now() + Math.random().toString(36).substr(2);
    return i;
  }

  removeComponent() {
    this.value.forEach((arr) => {
      let i = -1;
      arr.some((element, r) => {
        element.some((input, index) => {
          if (input.id == this.currentInput.id) {
            i = index;
            return true;
          }
        });
        if (i >= 0) {
          arr[r].splice(i, 1);
          return true;
        }
      });
    });
  }

  get formRules() {
    return this.$store.state.workflow.rules;
  }

  get formComponents() {
    return this.$store.state.workflow.formComponents;
  }

  get workflowInputs() {
    let arr = [];
    this.value.forEach((element) => {
      element.forEach((obj) => {
        arr.push(...obj);
      });
    });
    return arr;
  }

  @Watch("value")
  updateData1(value) {
    this.$emit("input", this.value);
    debugger;
    if(this.workflowInputs.length>0){
      this.$emit("on-validated", true);
    }else {
       this.$emit("on-validated", false);
    }
  }
  @Watch("currentInput", { deep: true })
  updateData(value) {
    this.$emit("input", this.value);
  }
}
</script>
<style scoped>
.form-split {
  height: calc(100% - 30px);
  min-height: 500px;
  border: 1px solid #dcdee2;
  background: #fff;
  width: 100%;
  position: absolute;
}
.attr-form .ivu-form-item {
  position: relative;
}

.attr-form .ivu-form-item .btn-right {
  position: absolute;
  padding: 0px 5px;
  top: -5px;
  right: 0px;
}
.form-split-pane {
  height: 100%;
}
.ivu-collapse {
  border-radius: 0px;
  border: 0px;
}
.form-row-btn {
  text-align: center;
  margin-top: 20px;
}
.form-split-pane.center .ivu-col {
  border: 1px dotted #ddd;
}
.form-split-pane.center .ivu-col > div {
  min-height: 30px;
}
.form-split-pane.center .ivu-row {
  padding: 5px;
}
.form-split-pane.center .ivu-row:hover,
.form-split-pane.center .ivu-row.active {
  border: 1px dotted #ff9900;
}

.form-split-pane.center .ivu-form,
.form-split-pane.right .ivu-form {
  height: 100%;
  position: relative;
  overflow-y: auto;
  padding: 10px;
}

.dragg-info {
  position: absolute;
  text-align: center;
  width: 100%;
  height: 30px;
  line-height: 30px;
  color: #ccc;
}
.form-tool {
  position: absolute;
  right: 0px;
  bottom: 0px;
  background: #ff9900;
  z-index: 1;
}
.form-tool i {
  padding: 2px;
  color: #fff;
  cursor: pointer;
}

.components-form {
  overflow-y: auto;
  max-height: 450px;

  margin-right: -22px;
  margin-left: -16px;
}
.draggable-item {
  position: relative;
  padding: 10px;
  background: #fff;
  cursor: move;
  border: 2px solid #fff;
  /* min-height: 30px; */
}

.draggable-item .form-tool {
  background: #409eff;
  right: -2px;
  bottom: -2px;
}

.draggable-item:hover,
.draggable-item.active {
  border: 2px solid #409eff;
  background: rgba(235, 245, 255, 0.5);
}
</style>