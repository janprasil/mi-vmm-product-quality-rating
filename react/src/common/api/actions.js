/* eslint-disable no-undef */
import 'isomorphic-fetch';

export const FETCH_REFERENCES_SUCCESS = 'FETCH_REFERENCES_SUCCESS';
export const FETCH_REFERENCES_ERROR = 'FETCH_REFERENCES_ERROR';
export const PUT_REFERENCE_SUCCESS = 'PUT_REFERENCE_SUCCESS';
export const DELETE_ALL_REFERENCES_SUCCESS = 'DELETE_ALL_REFERENCES_SUCCESS';

export const FETCH_DTW_SUCCESS = 'FETCH_DTW_SUCCESS';
export const FETCH_DTW_ERROR = 'FETCH_DTW_ERROR';

export const DELETE_ALL_SUCCESS = 'DELETE_ALL_SUCCESS';

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
  console.log(id, ct, ctl)
  return {
    type: 'PUT_REFERENCE',
    payload: fetch(`/webapi/reference?id=${id}&ct=${ct}&ctl=${ctl}`, { method: 'PUT' }).then(res => res.json())
  };
}

export function fetchImages() {
  return {
    type: 'FETCH_IMAGES',
    payload: fetch('/webapi/images').then(res => res.json())
  };
}

export function deleteAllImages() {
  return {
    type: 'DELETE_ALL_IMAGES',
    payload: fetch('/webapi/images/all', { method: 'DELETE' }).then(res => res.json())
  };
}

export function putImage(id, ct, ctl) {
  console.log(id, ct, ctl)
  return {
    type: 'PUT_IMAGE',
    payload: fetch(`/webapi/images?id=${id}&ct=${ct}&ctl=${ctl}`, { method: 'PUT' }).then(res => res.json())
  };
}

export function fetchDtw() {
  return {
    type: 'FETCH_DTW',
    payload: fetch('/webapi/contours/dtw').then(res => res.json())
  };
}

export function startProcessing() {
  return {
    type: 'START_PROCESSING',
    payload: fetch('/webapi/reference?id=2&ct=23.23&ctl=23', { method: 'PUT' }).then(res => res.json())
  }
}
