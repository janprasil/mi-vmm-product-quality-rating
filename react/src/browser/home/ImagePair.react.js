require('rc-slider/assets/index.css');

import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { putImage, putReference } from '../../common/api/actions';

@connect(state => ({
  sessionId: state.app.get('sessionId')
}), { putImage, putReference })
export default class ImagePair extends Component {

  static propTypes = {
    cannyTreshodLinking: RPT.number,
    cannyTreshold: RPT.number,
    contourUrl: RPT.string,
    id: RPT.string,
    imageUrl: RPT.string,
    isSelected: RPT.bool,
    onClick: RPT.func,
    pairType: RPT.oneOf(['reference', 'image']),
    putImage: RPT.func,
    putReference: RPT.func,
    sessionId: RPT.string,
  }

  render() {
    const { id, imageUrl, contourUrl, cannyTreshodLinking, cannyTreshold, putImage, putReference, pairType, sessionId, isSelected, onClick } = this.props;
    const wrapperStyle = isSelected ? { ...styles.imageWrapper, ...styles.selected } : styles.imageWrapper;
    return (
      <div style={wrapperStyle} onClick={onClick ? () => onClick() : () => {}}>
        <img src={imageUrl} style={styles.image} />
        <img src={`${contourUrl}?${Math.random()}`} style={styles.image} />
        <p>cannyThreshold</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, x, cannyTreshodLinking)
            : putImage(sessionId, id, x, cannyTreshodLinking)}
          defaultValue={cannyTreshold}
        />
        <p>cannyThresholdLinking</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, cannyTreshold, x)
            : putImage(sessionId, id, cannyTreshold, x)}
          defaultValue={cannyTreshodLinking}
        />
      </div>
    );
  }
}

const styles = {
  imageWrapper: {
    clear: 'both'
  },
  image: {
    width: '200px',
    marginLeft: '5px'
  },
  selected: {
    border: '3px solid blue'
  }
}
