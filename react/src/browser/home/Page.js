import Dtw from './Dtw.react';
import Images from './Images.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import References from './References.react';
import StatusBar from './StatusBar.react';
import { connect } from 'react-redux';
import { fetchSession, startProcessing } from '../../common/api/actions';

@connect(state => ({
  uploadPending: state.app.get('uploadPending'),
  selectedReference: state.app.get('selectedReference'),
  sessionId: state.app.get('sessionId'),
}), { fetchSession, startProcessing })
export default class Page extends Component {

  static propTypes = {
    fetchSession: RPT.func.isRequired,
    selectedReference: RPT.string,
    sessionId: RPT.string,
  }

  componentDidMount() {
    const { fetchSession } = this.props;
    fetchSession();
  }

  render() {
    const { startProcessing, selectedReference, sessionId } = this.props;

    return (
      <div>
        <StatusBar />
        <References />
        <Images />
        {selectedReference && sessionId 
          ? <button onClick={() => startProcessing(sessionId, selectedReference)}>Spustit výpočet</button>
          : <p>Před pokračováním nahrajte obrázky k porovnání a zvolte referenční obrázek.</p>
        }
        <Dtw />
      </div>
    );
  }
}
