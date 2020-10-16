const URL = process.env.NODE_ENV === 'production' ? 'https://yourdomain/' : 'http://localhost:21021/';
const UploadImageUrl=URL + "api/services/app/Attachment/UploadImage"
export {URL as default,UploadImageUrl};