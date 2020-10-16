<template>
  <div>
    <Select
      v-model="value"
      :multiple="true"
      filterable
      remote
      @on-change="onChange"
      :remote-method="onSearch"
      :loading="loading"
    >
      <Option
        v-for="option in options"
        :value="option.id"
        :key="option.id"
        :disabled="multiple==false && valueArray.length>0"
      >{{option.userName+"（"+option.fullName+"）"}}</Option>
    </Select>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "../lib/util";
import AbpBase from "../lib/abpbase";
import { UploadImageUrl } from "@/lib/url";
@Component
export default class SelectUsers extends AbpBase {
  @Prop({ default: null }) value: any;
  @Prop({ default: false }) multiple: boolean;
  loading = false;
  valueArray = [];
  initSuccessed = false;
  options = [];
  onChange(value) {
    if (this.multiple) {
      this.value = value;
    } else {
      this.value = value[value.length - 1];
    }
    this.$emit("input", this.value);
  }

  async searchUsers(param) {
    return await this.$store.dispatch({
      type: "user/searchList",
      data: param,
    });
  }

  async onSearch(query) {
    this.loading = true;
    let param = {
      keyword: query,
      userIds: this.value ? (this.multiple ? this.value : [this.value]) : null,
    };
    if (!param.userIds) {
      delete param.userIds;
    }
    this.options = await this.searchUsers(param);
    this.loading = false;
  }
  async mounted() {
    this.onSearch("");
  }
}
</script>
