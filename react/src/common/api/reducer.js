import * as actions from './actions';
import { Record, Map } from 'immutable';

const State = Record({
  contours: new Map(),
  dtw: new Map()
}, 'api');

const apiReducer = (state = new State(), action) => {
  switch (action.type) {
    case actions.FETCH_CONTOURS_START: {
      return state
        .setIn(['contours', 'error'], false)
        .setIn(['contours', 'pending'], true);
    }

    case actions.FETCH_CONTOURS_SUCCESS: {
      return state
        .setIn(['contours', 'data'], action.payload)
        .setIn(['contours', 'error'], false)
        .setIn(['contours', 'pending'], false);
    }

    case actions.FETCH_CONTOURS_ERROR: {
      return state
        .setIn(['contours', 'error'], true)
        .setIn(['contours', 'pending'], false);
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

    default:
      return state;

  }
};

export default apiReducer;
