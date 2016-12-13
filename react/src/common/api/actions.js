/* eslint-disable no-undef */
import 'isomorphic-fetch';

export const FETCH_CONTOURS_SUCCESS = 'FETCH_CONTOURS_SUCCESS';
export const FETCH_CONTOURS_ERROR = 'FETCH_CONTOURS_ERROR';

export const FETCH_DTW_SUCCESS = 'FETCH_DTW_SUCCESS';
export const FETCH_DTW_ERROR = 'FETCH_DTW_ERROR';

export const DELETE_ALL_SUCCESS = 'DELETE_ALL_SUCCESS';

export function fetchContours() {
  return {
    type: 'FETCH_CONTOURS',
    payload: fetch('/webapi/contours').then(res => res.json())
  };
}

export function fetchDtw() {
  return {
    type: 'FETCH_DTW',
    payload: fetch('/webapi/contours/dtw').then(res => res.json())
  };
}

export function deleteAll() {
  return {
    type: 'DELETE_ALL',
    payload: fetch('/webapi/contours', { method: 'DELETE' }).then(res => res.json())
  };
}
