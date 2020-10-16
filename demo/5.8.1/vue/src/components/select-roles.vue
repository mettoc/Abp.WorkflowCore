<template>
  <div>
    <Select
      v-model="value"
      :multiple="true"
      filterable
      @on-change="onChange"
      :loading="loading"
    >
      <Option
        v-for="option in options"
        :value="option.name"
        :key="option.name"
        :disabled="multiple == false && valueArray.length > 0"
        >{{ option.name + "（" + option.displayName + "）" }}</Option
      >
    </Select>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "../lib/util";
import AbpBase from "../lib/abpbase";
import { UploadImageUrl } from "@/lib/url";
@Component
export default class SelectRoles extends AbpBase {
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

  async mounted() {
    this.options = await this.$store.dispatch({
      type: "user/getRoles",
    });
  }
}
</script>
