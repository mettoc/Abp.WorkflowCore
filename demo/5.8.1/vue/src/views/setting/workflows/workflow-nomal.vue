<template>
  <Card dis-hover>
    <Row>
      <Col span="16" offset="6">
        <Form ref="nomalForm" label-position="top" :rules="workflowRule" :model="value">
          <Row :gutter="16">
            <Col span="8">
              <FormItem :label="L('Title')+'：'" prop="title">
                <Input v-model="value.title" :minlength="1"></Input>
              </FormItem>
            </Col>
            <Col span="8">
              <FormItem :label="L('分组')+'：'" prop="group">
                <AutoComplete v-model="value.group" :data="groups" :filter-method="filterMethod"></AutoComplete>
              </FormItem>
            </Col>
          </Row>
          <Row :gutter="16">
            <Col span="8">
              <FormItem :label="L('图标')+'：'">
                <AutoComplete v-model="value.icon" placeholder="input here">
                  <div class="icon-items">
                    <Option v-for="(item,key) in icons" :value="item" :key="key">
                      <Icon :type="item" size="24"></Icon>
                    </Option>
                  </div>
                </AutoComplete>
              </FormItem>
            </Col>
            <Col span="8">
              <FormItem :label="L('颜色')+'：'">
                <ColorPicker v-model="value.color" style="width:100%" :colors="colors" />
              </FormItem>
            </Col>
          </Row>
          <Row :gutter="16">
            <Col span="16">
              <FormItem :label="L('Description')+'：'">
                <Input type="textarea" v-model="value.description"></Input>
              </FormItem>
            </Col>
          </Row>
        </Form>
      </Col>
    </Row>
  </Card>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import WorkflowDefinition from "@/store/entities/workflow-definition";

@Component
export default class WorkFlowNomals extends AbpBase {
  @Prop({ type: Object, default: null }) value: WorkflowDefinition;
  groups = [];
  colors = [
    "#2d8cf0",
    "#5cadff",
    "#2b85e4",
    "#2db7f5",
    "#19be6b",
    "#ff9900",
    "#ed4014",
    "#17233d",
    "#515a6e",
    "#808695",
    "#dcdee2",
  ];
  icons = [
    "md-briefcase",
    "md-bus",
    "md-cafe",
    "md-call",
    "md-camera",
    "md-car",
    "md-cash",
    "md-clock",
    "md-contact",
    "md-desktop",
    "md-heart",
    "md-help",
    "md-plane",
    "md-print",
    "md-star",
  ];
  async getGeoups() {
    let g = await this.$store.dispatch({
      type: "workflow/getAllGroup",
    });
    this.groups.push(...g);
  }
  filterMethod(value, option) {
    if (option == null) return;
    return option.toUpperCase().indexOf(value.toUpperCase()) !== -1;
  }
  @Watch("value.title")
  @Watch("value.group")
  isValidate() {
    (this.$refs.nomalForm as any).validate(async (valid: boolean) => {
     
      this.$emit("on-validated", valid);
    });
  }
  workflowRule = {
    title: [
      {
        required: true,
        message: this.L("FieldIsRequired"),
      },
    ],
    group: [
      {
        required: true,
        message: this.L("FieldIsRequired"),
      },
    ],
  };
  async created() {
    await this.getGeoups();
    if (this.value) {
      this.value.color = this.colors[0];
    }
  }
}
</script>
<style scoped>
.icon-items {
  padding: 5px;
}

.ivu-select-item {
  display: inline-block;
  padding: 7px 15px;
}
</style>
