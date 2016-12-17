import { Record } from 'immutable';

const State = Record({
  error: null,
  location: null,
  uploadPending: false,
  sessionId: null,
  selectedReference: null,
  turns: 50,
  deviation: 0
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

    case 'FETCH_SESSION_SUCCESS': {
      const { sessionId } = action.payload;
      return state.set('sessionId', sessionId);
    }

    case 'SELECT_REFERENCE':
      return state.set('selectedReference', action.payload.key);

    case 'DELETE_ALL_REFERENCES_SUCCESS':
      return state.set('selectedReference', null);

    case 'CHANGE_SLIDER_VALUE': {
      const { slider, val } = action.payload;
      return state.set(slider, val);
    }

    default:
      return state;
  }
};

export default appReducer;
