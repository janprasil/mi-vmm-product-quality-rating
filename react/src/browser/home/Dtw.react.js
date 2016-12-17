import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  dtw: state.api.getIn(['dtw', 'data']),
  images: state.api.getIn(['images', 'data']),
}))
export default class Dtw extends Component {

  static propTypes = {
    dtw: RPT.array,
    images: RPT.array
  }

  render() {
    const { dtw, images } = this.props;
    if (!dtw) return null;

    const results = dtw.reduce((prev, r) => [...prev, [{
      label: images.getIn([r.object.imageId, 'imageUrl']).split('/').pop(),
      similarity: r.object.similarity,
      score: r.object.score,
      values: r.object.result.reduce((prev, val) =>
          [...prev, { x: parseInt(val.split(', ')[0], 10), y: parseInt(val.split(', ')[1], 10) }]
        , [])
    }]], []);

    return (
      <div>
        <h1>Výsledky</h1>
        {results.map(data =>
          <div style={styles.chart}>
            <p>{data[0].label}</p>
            <p>Similarity: {data[0].similarity}</p>
            <p>Score: {data[0].score}</p>
            <LineChart
              data={data}
              width={600}
              height={200}
            />
          </div>
        )}
      </div>
    );
  }
}

const styles = {
  chart: {
    marginTop: '20px',
    border: '1px solid grey'
  }
};
