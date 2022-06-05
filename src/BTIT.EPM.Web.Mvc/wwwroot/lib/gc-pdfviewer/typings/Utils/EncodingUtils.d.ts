declare function fasterBytesToBase64(arrayOrArrayBuffer: ArrayBuffer | ArrayLike<number> | ArrayBufferLike): string;
declare function bytesToBase64(bytes: any): string;
declare function base64ToBytes(dataURI: string): Uint8Array;
export { bytesToBase64, base64ToBytes, fasterBytesToBase64 };
