import { fetchImages, fetchReferences } from '../api/actions';
import { takeEvery } from 'redux-saga';
import { put } from 'redux-saga/effects';

function* fetchData() {
  yield put(fetchReferences());
  yield put(fetchImages());
}

export default function* saga() {
  yield* takeEvery('FILE_UPLOAD_MULTIPLE_FILE_UPLOAD_SUCCESS', fetchData);
}
