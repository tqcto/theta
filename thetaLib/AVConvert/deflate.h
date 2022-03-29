#pragma once

#define DLL_EXPORT __declspec(dllexport)

enum all_error {

	NOT_ERROR = 0,
	CAN_NOT_OPENED_FILE,
	CAN_NOT_GET_STREAM,
	VIDEO_STREAM_IS_NOT_FOUND,
	NOT_SUPPORTED_DECODER,
	CAN_NOT_ALLOCED_CODEC,
	CAN_NOT_COPYED_STREAMS,
	CAN_NOT_OPENED_CODEC,
	CAN_NOT_SEND_PACKET,
	CAN_NOT_OPENED_AVIO,
	CAN_NOT_MAKE_FORMAT,
	ENCODER_IS_NOT_FOUND,
	CAN_NOT_MAKE_CODEC,
	CAN_NOT_OPEN_CODEC,
	CAN_NOT_GET_NEW_STREAM,
	CAN_NOT_GET_PARAMETERS,
	CAN_NOT_WRITE_HEADER,
	CAN_NOT_SEND_FRAMES,
	CAN_NOT_WROTE_FRAME,
	NOT_WROTE_TRAILER,

};

typedef struct {

	AVRational time_base;
	AVFrame* data;

}decode_data;