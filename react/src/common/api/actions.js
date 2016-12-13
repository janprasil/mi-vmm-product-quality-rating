/* eslint-disable no-undef */
import 'isomorphic-fetch';

export const FETCH_CONTOURS_SUCCESS = 'FETCH_CONTOURS_SUCCESS';
export const FETCH_CONTOURS_ERROR = 'FETCH_CONTOURS_ERROR';

export function fetchContours() {
  return {
    type: 'FETCH_CONTOURS',
    payload: fetch('/webapi/contours').then(res => res.json())
  };
}
