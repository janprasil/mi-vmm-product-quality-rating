import * as actions from './actions';
import { Record, Map } from 'immutable';

const State = Record({
  images: new Map(),
  dtw: new Map(),
  references: new Map(),
}, 'api');

function transformData(data) {
  return data.reduce((prev, x) => prev.set(x.key, Map(x.object)), Map());
}

const apiReducer = (state = new State(), action) => {
  switch (action.type) {
    case actions.FETCH_REFERENCES_START: {
      return state
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pending'], true);
    }

    case actions.PUT_REFERENCE_SUCCESS:
    case actions.DELETE_ALL_REFERENCES_SUCCESS:
    case actions.FETCH_REFERENCES_SUCCESS: {
      return state
        .setIn(['references', 'data'], transformData(action.payload))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pending'], false);
    }

    case actions.FETCH_REFERENCES_ERROR: {
      return state
        .setIn(['references', 'error'], true)
        .setIn(['references', 'pending'], false);
    }

    case actions.FETCH_IMAGES_START: {
      return state
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pending'], true);
    }

    case actions.PUT_IMAGESSUCCESS:
    case actions.DELETE_ALL_IMAGES_SUCCESS:
    case actions.FETCH_IMAGES_SUCCESS: {
      return state
        .setIn(['images', 'data'], transformData(action.payload))
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pending'], false);
    }

    case actions.FETCH_IMAGES_ERROR: {
      return state
        .setIn(['images', 'error'], true)
        .setIn(['images', 'pending'], false);
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

    case 'FILE_UPLOAD_MULTIPLE_FILE_UPLOAD_SUCCESS': {

    }

    default:
      return state;

  }
};

export default apiReducer;
