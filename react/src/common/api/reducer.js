import * as actions from './actions';
import { Record, Map } from 'immutable';

const State = Record({
  images: new Map(),
  dtw: new Map(),
  references: new Map(),
}, 'api');

function transformData(data) {
  return data.reduce((prev, x) => prev.set(x.key, Map({ ...x.object, contourImageUrl: `${x.object.contourImageUrl}?${Math.random()}` })), Map());
}

const apiReducer = (state = new State(), action) => {
  switch (action.type) {
    case actions.FETCH_REFERENCES_START: {
      return state
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingFetch'], true);
    }

    case actions.FETCH_REFERENCES_SUCCESS: {
      return state
        .setIn(['references', 'data'], transformData(action.payload))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingFetch'], false);
    }

    case actions.FETCH_REFERENCES_ERROR: {
      return state
        .setIn(['references', 'error'], true)
        .setIn(['references', 'pendingFetch'], false);
    }

    case actions.PUT_REFERENCE_START: {
      return state
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingPut'], true);
    }

    case actions.PUT_REFERENCE_SUCCESS: {
      return state
        .setIn(['references', 'data'], transformData(action.payload))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingPut'], false);
    }

    case actions.DELETE_ALL_REFERENCES_START: {
      return state
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingDelete'], true);
    }

    case actions.DELETE_ALL_REFERENCES_SUCCESS: {
      return state
        .setIn(['references', 'data'], transformData(action.payload))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pendingDelete'], false)
        .set('dtw', new Map());
    }

    case actions.PUT_IMAGE_START: {
      return state
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pendingPut'], true);
    }

    case actions.PUT_IMAGE_SUCCESS: {
      return state
        .setIn(['images', 'data'], transformData(action.payload))
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pendingPut'], false);
    }

    case actions.DELETE_ALL_IMAGES_START: {
      return state
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pendingDelete'], true);
    }

    case actions.DELETE_ALL_IMAGES_SUCCESS: {
      return state
        .setIn(['images', 'data'], transformData(action.payload))
        .setIn(['images', 'error'], false)
        .setIn(['images', 'pendingDelete'], false)
        .set('dtw', new Map());
    }

    case actions.START_PROCESSING_START: {
      return state.set('dtw', new Map({ pending: true }));
    }

    case actions.START_PROCESSING_SUCCESS: {
      return state
        .setIn(['dtw', 'data'], action.payload)
        .setIn(['dtw', 'error'], false)
        .setIn(['dtw', 'pending'], false);
    }

    case 'FILE_UPLOAD_COMPLETE': {
      const { response } = action.payload;

      if (response.key) {
        return state
          .mergeIn(['images', 'data'], transformData(response.value))
          .setIn(['images', 'error'], false)
          .setIn(['images', 'pending'], false);
      }

      return state
        .mergeIn(['references', 'data'], transformData(response))
        .setIn(['references', 'error'], false)
        .setIn(['references', 'pending'], false);
    }

    default:
      return state;

  }
};

export default apiReducer;
