import Images from './Images.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import References from './References.react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { fetchReferences, startProcessing } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  referencesError: state.api.getIn(['references', 'error']),
  referencesPending: state.api.getIn(['references', 'pending']),
  dtw: state.api.getIn(['dtw', 'data']),
  dtwError: state.api.getIn(['dtw', 'error']),
  dtwPending: state.api.getIn(['dtw', 'pending']),
  uploadPending: state.app.get('uploadPending'),
  selectedReference: state.app.get('selectedReference'),
  sessionId: state.app.get('sessionId'),
}), { fetchReferences, startProcessing })
export default class Page extends Component {

  static propTypes = {
    dtw: RPT.array,
    dtwError: RPT.bool,
    dtwPending: RPT.bool,
    fetchReferences: RPT.func.isRequired,
    referencesError: RPT.bool,
    referencesPending: RPT.bool,
    selectedReference: RPT.string,
    sessionId: RPT.string,
    uploadPending: RPT.bool,
  }

  render() {
    const { referencesError, referencesPending, startProcessing, uploadPending, selectedReference, sessionId } = this.props;

    return (
      <div>
        <References />
        <Images />
        {referencesError && <p>Došlo k chybě. Určitě běží API?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {referencesPending && <p>Stahuji data a zpracovávám grafy...</p>}

        {selectedReference && sessionId 
          ? <button onClick={() => startProcessing(sessionId, selectedReference)}>Spustit výpočet</button>
          : <p>Před pokračováním nahrajte obrázky k porovnání a zvolte referenční obrázek.</p>
        }
      </div>
    );
  }
}
