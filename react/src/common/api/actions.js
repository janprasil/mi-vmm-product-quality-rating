/* eslint-disable no-undef */
import 'isomorphic-fetch';

export const FETCH_REFERENCES_START = 'FETCH_REFERENCES_START';
export const FETCH_REFERENCES_SUCCESS = 'FETCH_REFERENCES_SUCCESS';
export const FETCH_REFERENCES_ERROR = 'FETCH_REFERENCES_ERROR';

export const PUT_REFERENCE_START = 'PUT_REFERENCE_START';
export const PUT_REFERENCE_SUCCESS = 'PUT_REFERENCE_SUCCESS';
export const DELETE_ALL_REFERENCES_START = 'DELETE_ALL_REFERENCES_START';
export const DELETE_ALL_REFERENCES_SUCCESS = 'DELETE_ALL_REFERENCES_SUCCESS';


export const PUT_IMAGE_START = 'PUT_IMAGE_START';
export const PUT_IMAGE_SUCCESS = 'PUT_IMAGE_SUCCESS';

export const DELETE_ALL_IMAGES_START = 'DELETE_ALL_IMAGES_START';
export const DELETE_ALL_IMAGES_SUCCESS = 'DELETE_ALL_IMAGES_SUCCESS';

export const START_PROCESSING_START = 'START_PROCESSING_START';
export const START_PROCESSING_SUCCESS = 'START_PROCESSING_SUCCESS';
export const FETCH_SESSION_SUCCESS = 'FETCH_SESSION_SUCCESS';

export function fetchSession() {
  return {
    type: 'FETCH_SESSION',
    payload: fetch('/webapi/images/session').then(res => res.json())
  };
}

export function fetchReferences() {
  return {
    type: 'FETCH_REFERENCES',
    payload: fetch('/webapi/reference').then(res => res.json())
  };
}

export function deleteAllReferences() {
  return {
    type: 'DELETE_ALL_REFERENCES',
    payload: fetch('/webapi/reference/all', { method: 'DELETE' }).then(res => res.json())
  };
}

export function putReference(id, ct, ctl) {
  return {
    type: 'PUT_REFERENCE',
    payload: fetch(`/webapi/reference?id=${id}&ct=${ct}&ctl=${ctl}`, { method: 'PUT' }).then(res => res.json())
  };
}

export function deleteAllImages(sessionId) {
  return {
    type: 'DELETE_ALL_IMAGES',
    payload: fetch(`/webapi/images/all?sessionId=${sessionId}`, { method: 'DELETE' }).then(res => res.json())
  };
}

export function putImage(sessionId, id, ct, ctl) {
  return {
    type: 'PUT_IMAGE',
    payload: fetch(`/webapi/images?sessionId=${sessionId}&id=${id}&ct=${ct}&ctl=${ctl}`, { method: 'PUT' }).then(res => res.json())
  };
}

export function startProcessing(sessionId, referenceId) {
  return {
    type: 'START_PROCESSING',
    payload: fetch(`/webapi/dtw?sessionId=${sessionId}&referenceId=${referenceId}&turns=50`).then(res => res.json())
  };
}
