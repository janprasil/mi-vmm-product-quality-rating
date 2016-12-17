import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';

@connect(state => ({
  dtwPending: state.api.getIn(['dtw', 'pending']),
  imagesPendingDelete: state.api.getIn(['images', 'pendingDelete']),
  imagesPendingPut: state.api.getIn(['images', 'pendingPut']),
  referencesError: state.api.getIn(['references', 'error']),
  referencesPendingDelete: state.api.getIn(['references', 'pendingDelete']),
  referencesPendingFetch: state.api.getIn(['references', 'pendingFetch']),
  referencesPendingPut: state.api.getIn(['references', 'pendingPut']),
  uploadPending: state.app.get('uploadPending')
}))
export default class StatusBar extends Component {

  static propTypes = {
    dtwPending: RPT.bool,
    imagesPendingDelete: RPT.bool,
    imagesPendingPut: RPT.bool,
    referencesError: RPT.bool,
    referencesPendingDelete: RPT.bool,
    referencesPendingFetch: RPT.bool,
    referencesPendingPut: RPT.bool,
    uploadPending: RPT.bool
  }

  render() {
    const {
      dtwPending,
      referencesError,
      referencesPendingFetch,
      referencesPendingPut,
      referencesPendingDelete,
      imagesPendingPut,
      imagesPendingDelete,
      uploadPending,
    } = this.props;

    return (
      <div style={style.wrapper}>
        {!process.env.APP_URL && <p>Není nastavena proměnná APP_URL!</p>}
        {referencesError && process.env.APP_URL && <p>Došlo k chybě. Určitě na {process.env.APP_URL} běží backend?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {referencesPendingFetch && <p>Stahuji referenční obrázky...</p>}
        {(referencesPendingPut || imagesPendingPut) && <p>Odesílám nastavené hodnoty...</p>}
        {(referencesPendingDelete || imagesPendingDelete) && <p>Mažu...</p>}
        {dtwPending && <p>Počítám a kreslím grafy...</p>}
      </div>
    );
  }
}

const style = {
  wrapper: {
    position: 'fixed',
    top: 0,
    left: 0,
    width: '100%',
    height: 'auto',
    overflow: 'auto',
    backgroundColor: '#bacddb',
    borderBottom: '2px solid #c9dded',
    zIndex: '9999999999',
    paddingLeft: '50px'
  }
}
