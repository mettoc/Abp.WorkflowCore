<template>
  <div>
    <div class="image-upload-list" v-for="(item,key) in value" :key="key">
      <template v-if="item.status === 'finished'">
        <img :src="item.url" />
        <div class="image-upload-list-cover">
          <Icon type="ios-eye-outline" @click.native="handleView(item)"></Icon>
          <Icon type="ios-trash-outline" v-if="!disabled" @click.native="handleRemove(key)"></Icon>
        </div>
      </template>
      <template v-else>
        <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
      </template>
    </div>
    <Upload
      ref="upload"
      :show-upload-list="false"
      :default-file-list="value"
      :on-success="handleSuccess"
      :format="['jpg','jpeg','png']"
      :max-size="2048"
      :disabled="disabled"
      :on-format-error="handleFormatError"
      :on-exceeded-size="handleMaxSize"
      :before-upload="handleBeforeUpload"
      v-if="!disabled"
      multiple
      type="drag"
      :action="uploadpath"
      style="display: inline-block;width:58px;"
    >
      <div style="width: 58px;height:58px;line-height: 58px;">
        <Icon type="ios-camera" size="20"></Icon>
      </div>
    </Upload>
    <Modal :title="L('图片预览')" v-model="visible">
      <img :src="image.url" v-if="visible" style="width: 100%" />
    </Modal>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "../lib/util";
import AbpBase from "../lib/abpbase";
import { UploadImageUrl } from "@/lib/url";
@Component
export default class UploadImages extends AbpBase {
  @Prop({ default: [] }) value: Array<any>;
  @Prop({ default: 5 }) maxImageCount: number;
  @Prop({ default: false }) disabled: boolean;
  image = {};
  visible = false;
  handleView(item) {
    this.image = item;
    this.visible = true;
  }
  uploadpath = UploadImageUrl;
  handleRemove(index) {
    this.value.splice(index, 1);
    this.$emit("input", this.value);
  }
  handleSuccess(res, file) {
    this.value.push({
      showProgress: file.showProgress,
      url: file.response.result,
      name: file.name,
      status: file.status,
      percentage: file.percentage,
      size: file.size,
    });
    this.$emit("input", this.value);
  }
  handleFormatError(file) {
    this.$Notice.warning({
      title: this.L("The file format is incorrect"),
      desc:
        "File format of " +
        file.name +
        " is incorrect, please select jpg or png.",
    });
  }
  handleMaxSize(file) {
    this.$Notice.warning({
      title: "图片大小超出限制",
      desc: "文件  " + file.name + " 太大了, 不能超过 2M.",
    });
  }
  handleBeforeUpload() {
    const check = this.value.length < this.maxImageCount;
    if (!check) {
      this.$Notice.warning({
        title: "最多能上传" + this.maxImageCount + "张图片",
      });
    }
    return check;
  }
  created(){
      if(!this.value){
          this.value=[];
      }
  }
}

</script>

<style scoped>
.image-upload-list {
  display: inline-block;
  width: 60px;
  height: 60px;
  text-align: center;
  line-height: 60px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.image-upload-list img {
  width: 100%;
  height: 100%;
}
.image-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.image-upload-list:hover .image-upload-list-cover {
  display: block;
}
.image-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>
