/* eslint-disable no-undef */
import 'isomorphic-fetch';

export const FETCH_REFERENCES_SUCCESS = 'FETCH_REFERENCES_SUCCESS';
export const FETCH_REFERENCES_ERROR = 'FETCH_REFERENCES_ERROR';

export const FETCH_DTW_SUCCESS = 'FETCH_DTW_SUCCESS';
export const FETCH_DTW_ERROR = 'FETCH_DTW_ERROR';

export const DELETE_ALL_SUCCESS = 'DELETE_ALL_SUCCESS';

export function fetchReferences() {
  return {
    type: 'FETCH_REFERENCES',
    payload: fetch('/webapi/reference').then(res => res.json())
  };
}

export function fetchDtw() {
  return {
    type: 'FETCH_DTW',
    payload: fetch('/webapi/contours/dtw').then(res => res.json())
  };
}

export function deleteAllReference() {
  return {
    type: 'DELETE_ALL',
    payload: fetch('/webapi/contours', { method: 'DELETE' }).then(res => res.json())
  };
}

export function startProcessing() {
  return {
    type: 'START_PROCESSING',
    payload: fetch('/webapi/reference?id=2&ct=23.23&ctl=23', { method: 'PUT' }).then(res => res.json())
  }
}
