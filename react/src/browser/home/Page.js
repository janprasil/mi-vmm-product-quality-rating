require('rc-slider/assets/index.css');

import Button from '../app/components/Button';
import Dtw from './Dtw.react';
import Images from './Images.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import References from './References.react';
import Slider from 'rc-slider';
import StatusBar from './StatusBar.react';
import { connect } from 'react-redux';
import { changeSliderValue } from '../../common/app/actions';
import { fetchSession, startProcessing } from '../../common/api/actions';

@connect(state => ({
  deviation: state.app.get('deviation'),
  dtwPending: state.api.getIn(['dtw', 'pending']),
  images: state.api.getIn(['images', 'data']),
  selectedReference: state.app.get('selectedReference'),
  sessionId: state.app.get('sessionId'),
  turns: state.app.get('turns'),
  uploadPending: state.app.get('uploadPending'),
}), { changeSliderValue, fetchSession, startProcessing })
export default class Page extends Component {

  static propTypes = {
    changeSliderValue: RPT.func,
    deviation: RPT.number,
    dtwPending: RPT.bool,
    fetchSession: RPT.func.isRequired,
    images: RPT.array,
    selectedReference: RPT.string,
    sessionId: RPT.string,
    startProcessing: RPT.func,
    turns: RPT.number,
  }

  componentDidMount() {
    const { fetchSession } = this.props;
    fetchSession();
  }

  render() {
    const { changeSliderValue, dtwPending, startProcessing, selectedReference, sessionId, turns, deviation, images } = this.props;

    return (
      <div>
        <StatusBar />
        <References />
        <div style={styles.space} />
        <Images />
        <div style={styles.space} />
        {selectedReference && sessionId && images && images.size > 0
          ? (
            <div>
              <p>Kolikrát bude obrázek při testování pootočen?</p>
              <Slider
                max={500}
                min={0}
                step={10}
                onAfterChange={(val) => changeSliderValue('turns', val)}
                defaultValue={50}
              />
              <p>Maximální odchylka</p>
              <Slider
                max={100}
                min={0}
                step={10}
                onAfterChange={(val) => changeSliderValue('deviation', val)}
                defaultValue={0}
              />
              <div style={styles.space} />
              <Button disabled={dtwPending} onClick={() => startProcessing(sessionId, selectedReference, turns, deviation)}>Spustit výpočet</Button>
            </div>
            )
          : <p>Před pokračováním nahrajte obrázky k porovnání a zvolte referenční obrázek.</p>
        }
        <Dtw />
      </div>
    );
  }
}

const styles = {
  space: {
    height: '50px',
    width: '100%'
  }
};
