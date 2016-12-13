import * as actions from './actions';
import { Record } from 'immutable';

const State = Record({
  error: null,
  location: null,
  uploadPending: false
}, 'app');

const appReducer = (state = new State(), action) => {
  // This is how we can handle all async actions rejections.
  if (action.type.endsWith('_ERROR')) {
    const error = action.payload;
    return state.set('error', error);
  }

  switch (action.type) {
    case 'FILE_UPLOAD_MULTIPLE_FILE_UPLOAD_START':
      return state.set('uploadPending', true);

    case 'FILE_UPLOAD_MULTIPLE_FILE_UPLOAD_SUCCESS':
      return state.set('uploadPending', false);

    default:
      return state;

  }
};

export default appReducer;
