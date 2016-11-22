import * as actions from './actions';
import { fromJS, Record, Map } from 'immutable';

const State = Record({
  contours: new Map()
}, 'api');

const apiReducer = (state = new State(), action) => {
  switch (action.type) {
    case actions.FETCH_CONTOURS_SUCCESS: {
      return state.setIn(['contours', 'data'], action.payload);
    }

    default:
      return state;

  }
};

export default apiReducer;
