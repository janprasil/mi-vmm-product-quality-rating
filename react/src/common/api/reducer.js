import * as actions from './actions';
import { Record, Map } from 'immutable';

const State = Record({
  contours: new Map(),
  dtw: new Map(),
  references: new Map(),
}, 'api');

function transformReferences(data) {
  return data.reduce((prev, x) => prev.set(x.key, Map(x.object)), Map());
}

const apiReducer = (state = new State(), action) => {
  switch (action.type) {
    case actions.FETCH_REFERENCES_START: {
      return state
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pending'], true);
    }

    case actions.FETCH_REFERENCES_SUCCESS: {
      return state
        .setIn(['references', 'data'], transformReferences(action.payload))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pending'], false);
    }

    case actions.FETCH_REFERENCES_ERROR: {
      return state
        .setIn(['references', 'error'], true)
        .setIn(['references', 'pending'], false);
    }

    case actions.FETCH_DTW_START: {
      return state
        .setIn(['dtw', 'error'], false)
        .setIn(['dtw', 'pending'], true);
    }

    case actions.FETCH_DTW_SUCCESS: {
      return state
        .setIn(['dtw', 'data'], action.payload)
        .setIn(['dtw', 'error'], false)
        .setIn(['dtw', 'pending'], false);
    }

    case actions.FETCH_DTW_ERROR: {
      return state
        .setIn(['dtw', 'error'], true)
        .setIn(['dtw', 'pending'], false);
    }

    case actions.DELETE_ALL_SUCCESS: {
      return state
        .deleteIn(['contours', 'data'])
        .deleteIn(['dtw', 'data']);
    }

    case 'FILE_UPLOAD_MULTIPLE_FILE_UPLOAD_SUCCESS': {

    }

    default:
      return state;

  }
};

export default apiReducer;
